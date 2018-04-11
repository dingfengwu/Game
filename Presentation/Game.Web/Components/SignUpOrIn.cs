using Game.Facade.Components;
using Game.Web.Factories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Web.Components
{
    public class SignUpOrInViewComponent: GameViewComponent
    {
        private readonly ICommonModelFactory _commonModelFactory;

        public SignUpOrInViewComponent(ICommonModelFactory commonModelFactory)
        {
            this._commonModelFactory = commonModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _commonModelFactory.PrepareSignUpOrInModel();
            return View(model);
        }
    }
}
