using FluentValidation.AspNetCore;
using Game.Base;
using Game.Base.Caching;
using Game.Base.Configuration;
using Game.Base.Data;
using Game.Base.Infrastructure;
using Game.Facade.FluentValidation;
using Game.Facade.Mvc.ModelBinding;
using Game.Services.Authentication;
using Game.Services.Logging;
using Game.Services.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;

namespace Game.Face.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        /// <returns>Configured service provider</returns>
        public static IServiceProvider ConfigureApplicationServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            //add GameConfig configuration parameters
            services.ConfigureStartupConfig<GameConfig>(configuration.GetSection("Game"));
            //add hosting configuration parameters
            services.ConfigureStartupConfig<HostingConfig>(configuration.GetSection("Hosting"));
            //add accessor to HttpContext
            services.AddHttpContextAccessor();

            //create, initialize and configure the engine
            var engine = EngineContext.Create();
            engine.Initialize(services);
            var serviceProvider = engine.ConfigureServices(services, configuration);

            if (DataSettingsHelper.DatabaseIsInstalled())
            {
                //implement schedule tasks
                //database is already installed, so start scheduled tasks
                TaskManager.Instance.Initialize();
                TaskManager.Instance.Start();

                //log application start
                EngineContext.Current.Resolve<ILogger>().Information("Application started", null, null);
            }

            return serviceProvider;
        }

        /// <summary>
        /// Create, bind and register as service the specified configuration parameters 
        /// </summary>
        /// <typeparam name="TConfig">Configuration parameters</typeparam>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Set of key/value application configuration properties</param>
        /// <returns>Instance of configuration parameters</returns>
        public static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //create instance of config
            var config = new TConfig();

            //bind it to the appropriate section of configuration
            configuration.Bind(config);

            //and register it as a service
            services.AddSingleton(config);

            return config;
        }

        /// <summary>
        /// Register HttpContextAccessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// Adds services required for anti-forgery support
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAntiForgery(this IServiceCollection services)
        {
            //override cookie name
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = ".Game.Antiforgery";
            });
        }

        /// <summary>
        /// Adds services required for application session state
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpSession(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.Name = ".Game.Session";
                options.Cookie.HttpOnly = true;
            });
        }
        

        /// <summary>
        /// Adds data protection services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddGameDataProtection(this IServiceCollection services)
        {
            //check whether to persist data protection in Redis
            var gameConfig = services.BuildServiceProvider().GetRequiredService<GameConfig>();
            if (gameConfig.RedisCachingEnabled && gameConfig.PersistDataProtectionKeysToRedis)
            {
                //store keys in Redis
                services.AddDataProtection().PersistKeysToRedis(
                    () =>
                    {
                        var redisConnectionWrapper = EngineContext.Current.Resolve<IRedisConnectionWrapper>();
                        return redisConnectionWrapper.GetDatabase();
                    }, RedisConfiguration.DataProtectionKeysName);
            }
            else
            {
                var dataProtectionKeysPath = CommonHelper.MapPath("~/App_Data/DataProtectionKeys");
                var dataProtectionKeysFolder = new DirectoryInfo(dataProtectionKeysPath);

                //configure the data protection system to persist keys to the specified directory
                services.AddDataProtection().PersistKeysToFileSystem(dataProtectionKeysFolder);
            }
        }

        /// <summary>
        /// Adds authentication service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddGameAuthentication(this IServiceCollection services)
        {
            //set default authentication schemes
            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = GameCookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = GameCookieAuthenticationDefaults.ExternalAuthenticationScheme;
            });

            //add main cookie authentication
            authenticationBuilder.AddCookie(GameCookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = GameCookieAuthenticationDefaults.CookiePrefix + GameCookieAuthenticationDefaults.AuthenticationScheme;
                options.Cookie.HttpOnly = true;
                options.LoginPath = GameCookieAuthenticationDefaults.LoginPath;
                options.AccessDeniedPath = GameCookieAuthenticationDefaults.AccessDeniedPath;
            });

            //add external authentication
            authenticationBuilder.AddCookie(GameCookieAuthenticationDefaults.ExternalAuthenticationScheme, options =>
             {
                 options.Cookie.Name = GameCookieAuthenticationDefaults.CookiePrefix + GameCookieAuthenticationDefaults.ExternalAuthenticationScheme;
                 options.Cookie.HttpOnly = true;
                 options.LoginPath = GameCookieAuthenticationDefaults.LoginPath;
                 options.AccessDeniedPath = GameCookieAuthenticationDefaults.AccessDeniedPath;
             });
        }

        /// <summary>
        /// Add and configure MVC for the application
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <returns>A builder for configuring MVC services</returns>
        public static IMvcBuilder AddGameMvc(this IServiceCollection services)
        {
            //add basic MVC feature
            var mvcBuilder = services.AddMvc();

            //use session temp data provider
            mvcBuilder.AddSessionStateTempDataProvider();

            //MVC now serializes JSON with camel case names by default, use this code to avoid it
            mvcBuilder.AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //add custom display metadata provider
            mvcBuilder.AddMvcOptions(options => options.ModelMetadataDetailsProviders.Add(new GameMetadataProvider()));

            //add custom model binder provider (to the top of the provider list)
            mvcBuilder.AddMvcOptions(options => options.ModelBinderProviders.Insert(0, new GameModelBinderProvider()));

            //add fluent validation
            mvcBuilder.AddFluentValidation(configuration => configuration.ValidatorFactoryType = typeof(GameValidatorFactory));

            return mvcBuilder;
        }
    }
}
