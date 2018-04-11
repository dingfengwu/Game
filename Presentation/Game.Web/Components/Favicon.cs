using Game.Facade.Components;
using Game.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace Game.Web.Components
{
    public class FaviconViewComponent : GameViewComponent
    {
        private readonly ICommonModelFactory _commonModelFactory;

        public FaviconViewComponent(ICommonModelFactory commonModelFactory)
        {
            this._commonModelFactory = commonModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _commonModelFactory.PrepareFaviconModel();
            if (string.IsNullOrEmpty(model.FaviconUrl))
                return Content("");
            return View(model);
        }
    }
}
