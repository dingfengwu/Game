using Game.Base;
using Game.Base.Configuration;
using Game.Base.Infrastructure;
using Game.Facade.Compression;
using Game.Face.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.Linq;

namespace Game.Face.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring common features and middleware on application startup
    /// </summary>
    public class GameCommonStartup : IGameStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            ProxiesServiceCollectionExtensions.AddEntityFrameworkProxies(services);

            //compression
            services.AddResponseCompression(options=> {
                options.EnableForHttps = true;
                options.MimeTypes= ResponseCompressionDefaults.MimeTypes.Concat(new [] { "image/png", "image/jpeg", "image/gif", "image/x-icon" });
            });

            //add options feature
            services.AddOptions();

            //add memory cache
            services.AddMemoryCache();

            //add distributed memory cache
            services.AddDistributedMemoryCache();
                        
            //add HTTP sesion state feature
            services.AddHttpSession();

            //add anti-forgery
            services.AddAntiForgery();

            //add localization
            services.AddLocalization();

            //add theme support
            services.AddThemes();

            //add gif resizing support
            //new PrettyGifs().Install(Config.Current);
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            var gameConfig = EngineContext.Current.Resolve<GameConfig>();
            
            //compression
            if (gameConfig.UseResponseCompression)
            {
                //gzip by default
                application.UseResponseCompression();

                //workaround with "vary" header,use only on 1.x
                //application.UseMiddleware<ResponseCompressionVaryWorkaroundMiddleware>();
            }

            //static files
            application.UseStaticFiles(new StaticFileOptions
            {
                //TODO duplicated code (below)
                OnPrepareResponse = ctx =>
                {
                    if (!string.IsNullOrEmpty(gameConfig.StaticFilesCacheControl))
                        ctx.Context.Response.Headers.Append(HeaderNames.CacheControl, gameConfig.StaticFilesCacheControl);
                }
            });

            //themes
            application.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Themes")),
                RequestPath = new PathString("/Themes"),
                OnPrepareResponse = ctx =>
                {
                    if (!string.IsNullOrEmpty(gameConfig.StaticFilesCacheControl))
                        ctx.Context.Response.Headers.Append(HeaderNames.CacheControl, gameConfig.StaticFilesCacheControl);
                }
            });
            
            //plugins
            //var staticFileOptions = new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Plugins")),
            //    RequestPath = new PathString("/Plugins"),
            //    OnPrepareResponse = ctx =>
            //    {
            //        if (!string.IsNullOrEmpty(gameConfig.StaticFilesCacheControl))
            //            ctx.Context.Response.Headers.Append(HeaderNames.CacheControl,
            //                gameConfig.StaticFilesCacheControl);
            //    }
            //};
            //application.UseStaticFiles(staticFileOptions);
            
            //add support for backups
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".bak"] = MimeTypes.ApplicationOctetStream;
            application.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "db_backups")),
                RequestPath = new PathString("/db_backups"),
                ContentTypeProvider = provider
            });

            //check whether requested page is keep alive page
            application.UseKeepAlive();

            //check whether database is installed
            application.UseInstallUrl();

            //use HTTP session
            application.UseSession();

            //use request localization
            application.UseRequestLocalization();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order
        {
            //common services should be loaded after error handlers
            get { return 100; }
        }
    }
}
