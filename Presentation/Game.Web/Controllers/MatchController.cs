using Game.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace Game.Web.Controllers
{
    public class MatchController : BasePublicController
    {
        IMatchModelFactory _matchModelFactory;

        public MatchController(IMatchModelFactory matchModelFactory)
        {
            this._matchModelFactory = matchModelFactory;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public IActionResult List(int gameId=0)
        {
            var models = _matchModelFactory.PrepareMatchModel(gameId, 500, 0, "", 0);

            return View(models);
        }
    }
}