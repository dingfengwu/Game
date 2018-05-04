using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Game.Base;
using Game.Base.Domain.Matches;
using Game.Services.Matches;
using Game.Web.Models.Matches;

namespace Game.Web.Factories
{
    public class MatchModelFactory : IMatchModelFactory
    {
        IMatchService _matchService;

        public MatchModelFactory(IMatchService matchService)
        {
            this._matchService = matchService;
        }

        public IPagedList<MatchListItem> PrepareMatchModel(
            int gameId,
            int pageSize, 
            int pageIndex, 
            string keyword, 
            int id)
        {
            var models = _matchService.GetAvailableMatch().Select(p => new MatchListItem
            {
                GameId=p.GameId,
                GameIcon = Path.Combine("/images/games/", p.GameIcon),
                MasterTeam = p.MasterTeam,
                MasterTeamIcon = Path.Combine("/images/game_teams/", p.MasterTeamIcon),
                MasterTeamId = p.MasterTeamId,
                MasterTeamRate = p.MasterTeamRate,
                MasterTeamScore = p.MasterTeamScore,
                MatchId = p.MatchId,
                MatchName = p.MatchName,
                MatchState = p.MatchState,
                MatchTimeLocal = p.MatchTimeLocal,
                SlaveTeam = p.SlaveTeam,
                SlaveTeamIcon = Path.Combine("/images/game_teams/", p.SlaveTeamIcon),
                SlaveTeamId = p.SlaveTeamId,
                SlaveTeamRate = p.SlaveTeamRate,
                SlaveTeamScore = p.SlaveTeamScore
            });

            //比赛类型
            if (gameId>0)
            {
                models = models.Where(p => p.GameId == gameId);
            }

            //筛选
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                models = models.Where(p => p.MasterTeam.Contains(keyword) || p.SlaveTeam.Contains(keyword) || p.MatchName.Contains(keyword));
            }
            if (id > 0)
            {
                models = models.Where(p => p.MatchId == id);
            }
            var recordCount = models.Count();

            //分页
            models = models.Skip(pageIndex * pageSize).Take(pageSize).ToList();


            var result = new PagedList<MatchListItem>(models, pageIndex, pageSize, recordCount);
            return result;
        }
    }
}
