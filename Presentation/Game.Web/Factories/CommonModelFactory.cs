using Game.Base;
using Game.Base.Caching;
using Game.Base.Domain.Customers;
using Game.Base.Domain.Localization;
using Game.Facade.Themes;
using Game.Facade.UI;
using Game.Services.Common;
using Game.Services.Customers;
using Game.Services.Localization;
using Game.Services.Security;
using Game.Services.Seo;
using Game.Web.Infrastructure.Cache;
using Game.Web.Models.Common;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Web.Factories
{
    /// <summary>
    /// Represents the common models factory
    /// </summary>
    public partial class CommonModelFactory : ICommonModelFactory
    {
        #region Fields

        private readonly IWorkContext _workContext;
        private readonly IWebHelper _webHelper;
        private readonly IPageHeadBuilder _pageHeadBuilder;
        private readonly IStaticCacheManager _cacheManager;
        private readonly ISitemapGenerator _sitemapGenerator;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly LocalizationSettings _localizationSettings;
        private readonly ILanguageService _languageService;
        private readonly IPermissionService _permissionService;
        private readonly IThemeContext _themeContext;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICustomerService _customerService;

        #endregion

        #region Ctor

        public CommonModelFactory(IWorkContext workContext,
            IWebHelper webHelper,
            IThemeContext themeContext,
            ISitemapGenerator sitemapGenerator,
            IStaticCacheManager cacheManager,
            IHostingEnvironment hostingEnvironment,
            LocalizationSettings localizationSettings,
            ILanguageService languageService,
            IPermissionService permissionService,
            IPageHeadBuilder pageHeadBuilder,
            IGenericAttributeService genericAttributeService,
            ICustomerService customerService)
        {
            this._workContext = workContext;
            this._webHelper = webHelper;
            this._hostingEnvironment = hostingEnvironment;
            this._localizationSettings = localizationSettings;
            this._languageService = languageService;
            this._cacheManager = cacheManager;
            this._sitemapGenerator = sitemapGenerator;
            this._permissionService = permissionService;
            this._pageHeadBuilder = pageHeadBuilder;
            this._themeContext = themeContext;
            this._genericAttributeService = genericAttributeService;
            this._customerService = customerService;
        }

        #endregion

        #region Utilities

        #endregion

        #region Methods

        /// <summary>
        /// Prepare the sitemap model
        /// </summary>
        /// <returns>Sitemap model</returns>
        public virtual SitemapModel PrepareSitemapModel()
        {
            var cacheKey = string.Format(ModelCacheEventConsumer.SITEMAP_PAGE_MODEL_KEY,
                _workContext.WorkingLanguage.Id,
                string.Join(",", _workContext.CurrentCustomer.GetCustomerRoleIds()));
            var cachedModel = _cacheManager.Get(cacheKey, () =>
            {
                var model = new SitemapModel
                {
                    
                };
                return model;
            });

            return cachedModel;
        }

        /// <summary>
        /// Prepare the logo model
        /// </summary>
        /// <returns>Logo model</returns>
        public virtual LogoModel PrepareLogoModel()
        {
            var model = new LogoModel
            {
                
            };

            var cacheKey = string.Format(ModelCacheEventConsumer.STORE_LOGO_PATH, _themeContext.WorkingThemeName, _webHelper.IsCurrentConnectionSecured());
            model.LogoPath = _cacheManager.Get(cacheKey, () =>
            {
                //use default logo
                var logo = $"{_webHelper.GetSiteLocation()}Themes/{_themeContext.WorkingThemeName}/Content/images/logo.png";
                return logo;
            });

            return model;
        }

        /// <summary>
        /// Prepare the language selector model
        /// </summary>
        /// <returns>Language selector model</returns>
        public virtual LanguageSelectorModel PrepareLanguageSelectorModel()
        {
            var availableLanguages = _cacheManager.Get(ModelCacheEventConsumer.AVAILABLE_LANGUAGES_MODEL_KEY, () =>
            {
                var result = _languageService
                    .GetAllLanguages()
                    .Select(x => new LanguageModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        FlagImageFileName = x.FlagImageFileName,
                    })
                    .ToList();
                return result;
            });

            var model = new LanguageSelectorModel
            {
                CurrentLanguageId = _workContext.WorkingLanguage.Id,
                AvailableLanguages = availableLanguages,
                UseImages = _localizationSettings.UseImagesForLanguageSelection
            };

            return model;
        }

        /// <summary>
        /// Prepare top menu model
        /// </summary>
        /// <returns>Top menu model</returns>
        public virtual TopMenuModel PrepareTopMenuModel()
        {
            var model = new TopMenuModel
            {
                
            };
            return model;
        }

        /// <summary>
        /// Prepare the footer model
        /// </summary>
        /// <returns>Footer model</returns>
        public virtual FooterModel PrepareFooterModel()
        {
            //model
            var model = new FooterModel
            {
                
            };

            return model;
        }
        /// <summary>
        /// Prepare the admin header links model
        /// </summary>
        /// <returns>Admin header links model</returns>
        public virtual AdminHeaderLinksModel PrepareAdminHeaderLinksModel()
        {
            var customer = _workContext.CurrentCustomer;

            var model = new AdminHeaderLinksModel
            {
                ImpersonatedCustomerName = customer.IsRegistered() ? customer.FormatUserName() : "",
                IsCustomerImpersonated = _workContext.OriginalCustomerIfImpersonated != null,
                DisplayAdminLink = _permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel),
                EditPageUrl = _pageHeadBuilder.GetEditPageUrl()
            };

            return model;
        }
        /// <summary>
        /// Get the sitemap in XML format
        /// </summary>
        /// <param name="id">Sitemap identifier; pass null to load the first sitemap or sitemap index file</param>
        /// <returns>Sitemap as string in XML format</returns>
        public virtual string PrepareSitemapXml(int? id)
        {
            var cacheKey = string.Format(ModelCacheEventConsumer.SITEMAP_SEO_MODEL_KEY, id,
                _workContext.WorkingLanguage.Id,
                string.Join(",", _workContext.CurrentCustomer.GetCustomerRoleIds()));
            var siteMap = _cacheManager.Get(cacheKey, () => _sitemapGenerator.Generate(id));
            return siteMap;
        }

        /// <summary>
        /// Prepare the favicon model
        /// </summary>
        /// <returns>Favicon model</returns>
        public virtual FaviconModel PrepareFaviconModel()
        {
            var model = new FaviconModel();
            var faviconFileName = "favicon.ico";
            model.FaviconUrl = _webHelper.GetSiteLocation() + faviconFileName;
            return model;
        }

        /// <summary>
        /// Get robots.txt file
        /// </summary>
        /// <returns>Robots.txt file as string</returns>
        public virtual string PrepareRobotsTextFile()
        {
            var sb = new StringBuilder();

            //if robots.custom.txt exists, let's use it instead of hard-coded data below
            var robotsFilePath = System.IO.Path.Combine(CommonHelper.MapPath("~/"), "robots.custom.txt");
            if (System.IO.File.Exists(robotsFilePath))
            {
                //the robots.txt file exists
                var robotsFileContent = System.IO.File.ReadAllText(robotsFilePath);
                sb.Append(robotsFileContent);
            }
            else
            {
                //doesn't exist. Let's generate it (default behavior)

                var disallowPaths = new List<string>
                {
                    "/admin",
                    "/bin/",
                    "/files/",
                    "/files/exportimport/",
                    "/country/getstatesbycountryid",
                    "/install",
                    "/setproductreviewhelpfulness",
                };
                var localizableDisallowPaths = new List<string>
                {
                    "/addproducttocart/catalog/",
                    "/addproducttocart/details/",
                    "/backinstocksubscriptions/manage",
                    "/boards/forumsubscriptions",
                    "/boards/forumwatch",
                    "/boards/postedit",
                    "/boards/postdelete",
                    "/boards/postcreate",
                    "/boards/topicedit",
                    "/boards/topicdelete",
                    "/boards/topiccreate",
                    "/boards/topicmove",
                    "/boards/topicwatch",
                    "/cart",
                    "/checkout",
                    "/checkout/billingaddress",
                    "/checkout/completed",
                    "/checkout/confirm",
                    "/checkout/shippingaddress",
                    "/checkout/shippingmethod",
                    "/checkout/paymentinfo",
                    "/checkout/paymentmethod",
                    "/clearcomparelist",
                    "/compareproducts",
                    "/compareproducts/add/*",
                    "/customer/avatar",
                    "/customer/activation",
                    "/customer/addresses",
                    "/customer/changepassword",
                    "/customer/checkusernameavailability",
                    "/customer/downloadableproducts",
                    "/customer/info",
                    "/deletepm",
                    "/emailwishlist",
                    "/inboxupdate",
                    "/newsletter/subscriptionactivation",
                    "/onepagecheckout",
                    "/order/history",
                    "/orderdetails",
                    "/passwordrecovery/confirm",
                    "/poll/vote",
                    "/privatemessages",
                    "/returnrequest",
                    "/returnrequest/history",
                    "/rewardpoints/history",
                    "/sendpm",
                    "/sentupdate",
                    "/shoppingcart/*",
                    "/storeclosed",
                    "/subscribenewsletter",
                    "/topic/authenticate",
                    "/viewpm",
                    "/uploadfilecheckoutattribute",
                    "/uploadfileproductattribute",
                    "/uploadfilereturnrequest",
                    "/wishlist",
                };

                const string newLine = "\r\n"; //Environment.NewLine
                sb.Append("User-agent: *");
                sb.Append(newLine);
                //sitemaps
                if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
                {
                    //URLs are localizable. Append SEO code
                    foreach (var language in _languageService.GetAllLanguages())
                    {
                        sb.AppendFormat("Sitemap: {0}/sitemap.xml", language.UniqueSeoCode);
                        sb.Append(newLine);
                    }
                }
                else
                {
                    //localizable paths (without SEO code)
                    sb.AppendFormat("Sitemap: sitemap.xml");
                    sb.Append(newLine);
                }

                //usual paths
                foreach (var path in disallowPaths)
                {
                    sb.AppendFormat("Disallow: {0}", path);
                    sb.Append(newLine);
                }
                //localizable paths (without SEO code)
                foreach (var path in localizableDisallowPaths)
                {
                    sb.AppendFormat("Disallow: {0}", path);
                    sb.Append(newLine);
                }
                if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
                {
                    //URLs are localizable. Append SEO code
                    foreach (var language in _languageService.GetAllLanguages())
                    {
                        foreach (var path in localizableDisallowPaths)
                        {
                            sb.AppendFormat("Disallow: /{0}{1}", language.UniqueSeoCode, path);
                            sb.Append(newLine);
                        }
                    }
                }

                //load and add robots.txt additions to the end of file.
                var robotsAdditionsFile = System.IO.Path.Combine(CommonHelper.MapPath("~/"), "robots.additions.txt");
                if (System.IO.File.Exists(robotsAdditionsFile))
                {
                    var robotsFileContent = System.IO.File.ReadAllText(robotsAdditionsFile);
                    sb.Append(robotsFileContent);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public SignUpOrInModel PrepareSignUpOrInModel()
        {
            var customer = _workContext.CurrentCustomer;
            var userIcon = customer.GetAttribute<string>(SystemCustomerAttributeNames.UserIcon);
            var isGuest = _customerService.IsGuest(customer.Id);
            var model = new SignUpOrInModel()
            {
                RegisteredUser = !isGuest,
                UserId = customer.Id,
                UserIcon = userIcon
            };
            
            return model;
        }

        #endregion
    }
}
