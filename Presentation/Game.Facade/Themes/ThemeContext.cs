using System;
using System.Linq;
using Game.Base;
using Game.Base.Domain;
using Game.Base.Domain.Customers;
using Game.Services.Common;
using Game.Services.Themes;

namespace Game.Facade.Themes
{
    /// <summary>
    /// Represents the theme context implementation
    /// </summary>
    public partial class ThemeContext : IThemeContext
    {
        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IThemeProvider _themeProvider;
        private readonly IWorkContext _workContext;
        
        private string _cachedThemeName;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="genericAttributeService">Generic attribute service</param>
        /// <param name="storeContext">Store context</param>
        /// <param name="themeProvider">Theme provider</param>
        /// <param name="workContext">Work context</param>
        /// <param name="storeInformationSettings">Store information settings</param>
        public ThemeContext(IGenericAttributeService genericAttributeService,
            IThemeProvider themeProvider,
            IWorkContext workContext)
        {
            this._genericAttributeService = genericAttributeService;
            this._themeProvider = themeProvider;
            this._workContext = workContext;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get or set current theme system name
        /// </summary>
        public string WorkingThemeName
        {
            get
            {
                if (!string.IsNullOrEmpty(_cachedThemeName))
                    return _cachedThemeName;

                var themeName = string.Empty;
                
                //ensure that this theme exists
                if (!_themeProvider.ThemeExists(themeName))
                {
                    //if it does not exist, try to get the first one
                    themeName = _themeProvider.GetThemes().FirstOrDefault()?.SystemName 
                        ?? throw new Exception("No theme could be loaded");
                }
                
                //cache theme system name
                this._cachedThemeName = themeName;

                return themeName;
            }
            set
            {
                //save selected by customer theme system name
                _genericAttributeService.SaveAttribute(_workContext.CurrentCustomer, 
                    SystemCustomerAttributeNames.WorkingThemeName, value);

                //clear cache
                this._cachedThemeName = null;
            }
        }

        #endregion
    }
}
