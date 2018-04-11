using System;
using Microsoft.AspNetCore.Mvc;
using Game.Base;
using Game.Base.Domain;
using Game.Base.Domain.Customers;
using Game.Services.Common;
using Game.Facade.Components;
using Game.Base.Domain.Common;

namespace Game.Web.Components
{
    public class EuCookieLawViewComponent : GameViewComponent
    {
        private readonly IWorkContext _workContext;
        private readonly CommonSettings _commonSettings;

        public EuCookieLawViewComponent(IWorkContext workContext,CommonSettings commonSettings)
        {
            this._workContext = workContext;
            this._commonSettings = commonSettings;
        }

        public IViewComponentResult Invoke()
        {
            if (!_commonSettings.DisplayEuCookieLawWarning)
                //disabled
                return Content("");

            //ignore search engines because some pages could be indexed with the EU cookie as description
            if (_workContext.CurrentCustomer.IsSearchEngineAccount())
                return Content("");

            if (_workContext.CurrentCustomer.GetAttribute<bool>(SystemCustomerAttributeNames.EuCookieLawAccepted))
                //already accepted
                return Content("");

            //ignore notification?
            //right now it's used during logout so popup window is not displayed twice
            if (TempData["game.IgnoreEuCookieLawWarning"] != null && Convert.ToBoolean(TempData["game.IgnoreEuCookieLawWarning"]))
                return Content("");

            return View();
        }
    }
}
