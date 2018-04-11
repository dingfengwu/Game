using Microsoft.AspNetCore.Mvc;
using Game.Web.Factories;
using Game.Facade.Components;

namespace Game.Web.Components
{
    public class TopMenuViewComponent : GameViewComponent
    {
        private readonly ICommonModelFactory _catalogModelFactory;

        public TopMenuViewComponent(ICommonModelFactory catalogModelFactory)
        {
            this._catalogModelFactory = catalogModelFactory;
        }

        public IViewComponentResult Invoke(int? productThumbPictureSize)
        {
            var model = _catalogModelFactory.PrepareTopMenuModel();
            return View(model);
        }
    }
}
