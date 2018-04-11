using Game.Facade.Components;
using Game.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace Game.Web.Components
{
    public class FooterViewComponent : GameViewComponent
    {
        private readonly ICommonModelFactory _commonModelFactory;

        public FooterViewComponent(ICommonModelFactory commonModelFactory)
        {
            this._commonModelFactory = commonModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _commonModelFactory.PrepareFooterModel();
            return View(model);
        }
    }
}
