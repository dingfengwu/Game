using Game.Base;
using Game.Base.Domain.Matches;
using Game.Services.Helpers;
using Game.Services.Matches;
using Game.Web.Factories;
using Game.Web.Models.Matches;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Game.Web.Areas.Admin.Controllers
{
    public class MatchController : BaseAdminController
    {
        readonly IMatchModelFactory _matchModelFactory;
        readonly IMatchService _matchService;
        readonly IWorkContext _workContext;
        readonly IDateTimeHelper _dateTimerHelper;

        public MatchController(IMatchModelFactory matchModelFactory,
            IMatchService matchService,IWorkContext workContext,
            IDateTimeHelper dateTimerHelper
            )
        {
            this._matchModelFactory = matchModelFactory;
            this._matchService = matchService;
            this._workContext = workContext;
            this._dateTimerHelper = dateTimerHelper;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(List));
        }

        public IActionResult List()
        {
            var models = _matchModelFactory.PrepareMatchModel(0, 500, 0, "", 0);

            return View(models);
        }

        public IActionResult Edit(int id=0)
        {
            MatchPostModel match =null ;
            if (id > 0)
            {
                var model = _matchService.GetMatch(id);
                if (model != null)
                {
                    match = new MatchPostModel();
                    match.Enabled = model.Enabled;
                    match.GameId = model.GameId;
                    match.LiveUrl = model.LiveUrl;
                    match.MasterTeamId = model.MasterTeamId;
                    match.MasterTeamRate = model.MasterTeamRate;
                    match.MasterTeamScore = model.MasterTeamScore;
                    match.MatchName = model.MatchName;
                    match.MatchTimeLocal = _dateTimerHelper.ConvertToUserTime(model.MatchTimeUtc, DateTimeKind.Utc);
                    match.Memo = model.Memo;
                    match.SlaveTeamId = model.SlaveTeamId;
                    match.SlaverTeamRate = model.SlaverTeamRate;
                    match.SlaverTeamScore = model.SlaverTeamScore;
                }
            }
            return View(match);
        }

        [HttpPost]
        public IActionResult Edit(MatchPostModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Match match = null;
                    if (model.Id > 0)
                    {
                        match = _matchService.GetMatch(model.Id);
                        match.Enabled = model.Enabled;
                        match.GameId = model.GameId;
                        match.LiveUrl = model.LiveUrl;
                        match.MasterTeamId = model.MasterTeamId;
                        match.MasterTeamRate = model.MasterTeamRate;
                        match.MasterTeamScore = model.MasterTeamScore;
                        match.MatchName = model.MatchName;
                        match.MatchTimeUtc = model.MatchTimeLocal.ToUniversalTime();
                        match.Memo = model.Memo;
                        match.SlaveTeamId = model.SlaveTeamId;
                        match.SlaverTeamRate = model.SlaverTeamRate;
                        match.SlaverTeamScore = model.SlaverTeamScore;
                        match.UpdateTimeUtc = DateTime.UtcNow;
                        match.UpdateUserId = _workContext.CurrentCustomer.Id;
                        _matchService.UpdateMatch(match);
                    }
                    else
                    {
                        match = new Match
                        {
                            Enabled = model.Enabled,
                            GameId = model.GameId,
                            LiveUrl = model.LiveUrl,
                            MasterTeamId = model.MasterTeamId,
                            MasterTeamRate = model.MasterTeamRate,
                            MasterTeamScore = model.MasterTeamScore,
                            MatchName = model.MatchName,
                            MatchTimeUtc = model.MatchTimeLocal.ToUniversalTime(),
                            Memo = model.Memo,
                            SlaveTeamId = model.SlaveTeamId,
                            SlaverTeamRate = model.SlaverTeamRate,
                            SlaverTeamScore = model.SlaverTeamScore,
                            CreateTimeUtc = DateTime.UtcNow,
                            CreateUserId = _workContext.CurrentCustomer.Id
                        };
                        
                        _matchService.InsertMatch(match);
                    }
                    return RedirectToAction("List");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("applicationError", ex.Message);
            }

            return View(model);
        }
    }
}