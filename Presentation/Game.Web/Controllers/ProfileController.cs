using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Facade.Mvc.Filters;
using Game.Facade.Security;
using Microsoft.AspNetCore.Mvc;

namespace Game.Web.Controllers
{
    [AuthorizeRegisted]
    [HttpsRequirement(SslRequirement.Yes)]
    public class ProfileController : BasePublicController
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}