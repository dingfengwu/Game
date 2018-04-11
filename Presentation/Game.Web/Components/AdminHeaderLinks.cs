using Game.Facade.Components;
using Game.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace Game.Web.Components
{
    public class AdminHeaderLinksViewComponent : GameViewComponent
    {
        private readonly ICommonModelFactory _commonModelFactory;

        public AdminHeaderLinksViewComponent(ICommonModelFactory commonModelFactory)
        {
            this._commonModelFactory = commonModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _commonModelFactory.PrepareAdminHeaderLinksModel();
            return View(model);
        }
    }
}
