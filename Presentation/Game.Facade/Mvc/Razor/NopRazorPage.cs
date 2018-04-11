using Game.Base;
using Game.Base.Infrastructure;
using Game.Facade.Localization;
using Game.Facade.Themes;
using Game.Services.Localization;
using Game.Services.Themes;

namespace Game.Facade.Mvc.Razor
{
    /// <summary>
    /// Web view page
    /// </summary>
    /// <typeparam name="TModel">Model</typeparam>
    public abstract class GameRazorPage<TModel> : Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel>
    {
        private ILocalizationService _localizationService;
        private Localizer _localizer;

        /// <summary>
        /// Get a localized resources
        /// </summary>
        public Localizer T
        {
            get
            {
                if (_localizationService == null)
                    _localizationService = EngineContext.Current.Resolve<ILocalizationService>();

                if (_localizer == null)
                {
                    _localizer = (format, args) =>
                    {
                        var resFormat = _localizationService.GetResource(format);
                        if (string.IsNullOrEmpty(resFormat))
                        {
                            return new LocalizedString(format);
                        }
                        return new LocalizedString((args == null || args.Length == 0)
                            ? resFormat
                            : string.Format(resFormat, args));
                    };
                }
                return _localizer;
            }
        }

        /// <summary>
        /// Return a value indicating whether the working language and theme support RTL (right-to-left)
        /// </summary>
        /// <returns></returns>
        public bool ShouldUseRtlTheme()
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var supportRtl = workContext.WorkingLanguage.Rtl;
            if (supportRtl)
            {
                //ensure that the active theme also supports it
                var themeProvider = EngineContext.Current.Resolve<IThemeProvider>();
                var themeContext = EngineContext.Current.Resolve<IThemeContext>();
                supportRtl = themeProvider.GetThemeBySystemName(themeContext.WorkingThemeName)?.SupportRtl ?? false;
            }
            return supportRtl;
        }
    }

    /// <summary>
    /// Web view page
    /// </summary>
    public abstract class GameRazorPage : GameRazorPage<dynamic>
    {
    }
}