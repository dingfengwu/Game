using Microsoft.AspNetCore.Mvc;
using Game.Web.Factories;
using Game.Facade.Components;

namespace Game.Web.Components
{
    public class LogoViewComponent : GameViewComponent
    {
        private readonly ICommonModelFactory _commonModelFactory;

        public LogoViewComponent(ICommonModelFactory commonModelFactory)
        {
            this._commonModelFactory = commonModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _commonModelFactory.PrepareLogoModel();
            return View(model);
        }
    }
}
