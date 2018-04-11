using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Base.Domain.Matches;
using Game.Facade.Controllers;
using Game.Web.Models.Matches;
using Microsoft.AspNetCore.Mvc;

namespace Game.Web.Controllers
{
    public class HomeController : BasePublicController
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Match");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
