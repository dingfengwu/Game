using Game.Base;
using Game.Base.Domain.Common;
using Game.Base.Domain.Customers;
using Game.Base.Domain.Localization;
using Game.Facade.Localization;
using Game.Facade.Mvc.Filters;
using Game.Facade.Security;
using Game.Facade.Security.Captcha;
using Game.Facade.Themes;
using Game.Services.Common;
using Game.Services.Localization;
using Game.Services.Logging;
using Game.Web.Controllers;
using Game.Web.Factories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Game.WebControllers
{
    public partial class CommonController : BasePublicController
    {
        #region Fields

        private readonly ICommonModelFactory _commonModelFactory;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IThemeContext _themeContext;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILogger _logger;
        
        private readonly CommonSettings _commonSettings;
        private readonly LocalizationSettings _localizationSettings;
        private readonly CaptchaSettings _captchaSettings;
        
        #endregion
        
        #region Ctor

        public CommonController(ICommonModelFactory commonModelFactory,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IWorkContext workContext,
            IThemeContext themeContext,
            IGenericAttributeService genericAttributeService,
            ICustomerActivityService customerActivityService,
            ILogger logger,
            CommonSettings commonSettings,
            LocalizationSettings localizationSettings,
            CaptchaSettings captchaSettings)
        {
            this._commonModelFactory = commonModelFactory;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._themeContext = themeContext;
            this._genericAttributeService = genericAttributeService;
            this._customerActivityService = customerActivityService;
            this._logger = logger;
            this._commonSettings = commonSettings;
            this._localizationSettings = localizationSettings;
            this._captchaSettings = captchaSettings;
        }

        #endregion

        #region Methods
        
        //page not found
        public virtual IActionResult PageNotFound()
        {
            if (_commonSettings.Log404Errors)
            {
                var statusCodeReExecuteFeature = HttpContext?.Features?.Get<IStatusCodeReExecuteFeature>();
                //TODO add locale resource
                _logger.Error($"Error 404. The requested page ({statusCodeReExecuteFeature?.OriginalPath}) was not found", 
                    customer: _workContext.CurrentCustomer);
            }

            Response.StatusCode = 404;
            Response.ContentType = "text/html";

            return View();
        }
        
        public virtual IActionResult SetLanguage(int langid, string returnUrl = "")
        {
            var language = _languageService.GetLanguageById(langid);
            if (!language?.Published ?? false)
                language = _workContext.WorkingLanguage;

            //home page
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            //language part in URL
            if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                //remove current language code if it's already localized URL
                if (returnUrl.IsLocalizedUrl(this.Request.PathBase, true, out Language _))
                    returnUrl = returnUrl.RemoveLanguageSeoCodeFromUrl(this.Request.PathBase, true);

                //and add code of passed language
                returnUrl = returnUrl.AddLanguageSeoCodeToUrl(this.Request.PathBase, true, language);
            }

            _workContext.WorkingLanguage = language;

            return Redirect(returnUrl);
        }
        
        //sitemap page
        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult Sitemap()
        {
            if (!_commonSettings.SitemapEnabled)
                return RedirectToRoute("HomePage");

            var model = _commonModelFactory.PrepareSitemapModel();
            return View(model);
        }

        //SEO sitemap page
        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult SitemapXml(int? id)
        {
            if (!_commonSettings.SitemapEnabled)
                return RedirectToRoute("HomePage");

            var siteMap = _commonModelFactory.PrepareSitemapXml(id);
            return Content(siteMap, "text/xml");
        }
        
        [HttpPost]
        public virtual IActionResult EuCookieLawAccept()
        {
            //save setting
            _genericAttributeService.SaveAttribute(_workContext.CurrentCustomer, SystemCustomerAttributeNames.EuCookieLawAccepted, true);
            return Json(new { stored = true });
        }

        //robots.txt file
        public virtual IActionResult RobotsTextFile()
        {
            var robotsFileContent = _commonModelFactory.PrepareRobotsTextFile();
            return Content(robotsFileContent, MimeTypes.TextPlain);
        }

        public virtual IActionResult GenericUrl()
        {
            //seems that no entity was found
            return InvokeHttp404();
        }
        
        //helper method to redirect users. Workaround for GenericPathRoute class where we're not allowed to do it
        public virtual IActionResult InternalRedirect(string url, bool permanentRedirect)
        {
            //ensure it's invoked from our GenericPathRoute class
            if (HttpContext.Items["game.RedirectFromGenericPathRoute"] == null ||
                !Convert.ToBoolean(HttpContext.Items["game.RedirectFromGenericPathRoute"]))
            {
                url = Url.RouteUrl("HomePage");
                permanentRedirect = false;
            }

            //home page
            if (string.IsNullOrEmpty(url))
            {
                url = Url.RouteUrl("HomePage");
                permanentRedirect = false;
            }

            //prevent open redirection attack
            if (!Url.IsLocalUrl(url))
            {
                url = Url.RouteUrl("HomePage");
                permanentRedirect = false;
            }

            if (permanentRedirect)
                return RedirectPermanent(url);

            return Redirect(url);
        }

        #endregion
    }
}