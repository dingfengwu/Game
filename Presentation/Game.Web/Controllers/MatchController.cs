using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Base.Domain.Matches;
using Game.Web.Models.Matches;
using Microsoft.AspNetCore.Mvc;

namespace Game.Web.Controllers
{
    public class MatchController : BasePublicController
    {
        public IActionResult Index()
        {
            var models = new List<MatchListItem>()
            {
                new MatchListItem
                {
                    MasterTeam ="G2",
                    MasterTeamIcon ="/images/game_teams/1.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.92m,

                    SlaveTeam="LDLC",
                    SlaveTeamIcon="/images/game_teams/2.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.98m,

                    GameIcon="/images/games/cs.png",
                    MatchName="ESL职业联赛欧洲区小组赛",
                    MatchTimeUtc=DateTime.Parse("2017-04-06 4:00")
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="ESL职业联赛欧洲区小组赛",
                    MatchTimeUtc=DateTime.Parse("2017-04-06 8:00"),

                    MatchState= MatchState.Guessing
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="测试团队I",
                    MatchTimeUtc=DateTime.Parse("2017-04-06 8:00")
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="测试团队-龙虎队",
                    MatchTimeUtc=DateTime.Parse("2017-04-06 8:00")
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="测试团队II",
                    MatchTimeUtc=DateTime.Parse("2017-04-08 12:00"),

                    MatchState= MatchState.Guessing
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="ESL职业联赛欧洲区小组赛",
                    MatchTimeUtc=DateTime.Parse("2017-04-07 16:00")
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="测试团队-野狼队",
                    MatchTimeUtc=DateTime.Parse("2017-04-07 12:00"),

                    MatchState= MatchState.Guessing
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="ESL职业联赛欧洲区小组赛",
                    MatchTimeUtc=DateTime.Parse("2017-04-04 23:00")
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="测试团队-小鸟队",
                    MatchTimeUtc=DateTime.Parse("2017-04-05 21:00"),

                    MatchState= MatchState.Guessing
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="ESL职业联赛欧洲区小组赛",
                    MatchTimeUtc=DateTime.Parse("2017-04-04 12:00")
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="测试团队-XX团",
                    MatchTimeUtc=DateTime.Parse("2017-04-02 6:00"),

                    MatchState= MatchState.StopGuess
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="ESL职业联赛欧洲区小组赛",
                    MatchTimeUtc=DateTime.Parse("2017-04-05 3:00"),

                    MatchState= MatchState.StopGuess
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="ESL职业联赛欧洲区小组赛",
                    MatchTimeUtc=DateTime.Parse("2017-04-02 1:00")
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="ESL职业联赛欧洲区小组赛",
                    MatchTimeUtc=DateTime.Parse("2017-04-07 2:00"),

                    MatchState= MatchState.StopGuess
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="测试团队-XX团",
                    MatchTimeUtc=DateTime.Parse("2017-04-02 6:00"),

                    MatchState= MatchState.StopGuess
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="ESL职业联赛欧洲区小组赛",
                    MatchTimeUtc=DateTime.Parse("2017-04-05 3:00"),

                    MatchState= MatchState.StopGuess
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="ESL职业联赛欧洲区小组赛",
                    MatchTimeUtc=DateTime.Parse("2017-04-02 1:00")
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="ESL职业联赛欧洲区小组赛",
                    MatchTimeUtc=DateTime.Parse("2017-04-07 2:00"),

                    MatchState= MatchState.StopGuess
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="测试团队-XX团",
                    MatchTimeUtc=DateTime.Parse("2017-04-02 6:00"),

                    MatchState= MatchState.StopGuess
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="ESL职业联赛欧洲区小组赛",
                    MatchTimeUtc=DateTime.Parse("2017-04-05 3:00"),

                    MatchState= MatchState.StopGuess
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="ESL职业联赛欧洲区小组赛",
                    MatchTimeUtc=DateTime.Parse("2017-04-02 1:00")
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="ESL职业联赛欧洲区小组赛",
                    MatchTimeUtc=DateTime.Parse("2017-04-07 2:00"),

                    MatchState= MatchState.StopGuess
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="测试团队-XX团",
                    MatchTimeUtc=DateTime.Parse("2017-04-02 6:00"),

                    MatchState= MatchState.StopGuess
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="ESL职业联赛欧洲区小组赛",
                    MatchTimeUtc=DateTime.Parse("2017-04-05 3:00"),

                    MatchState= MatchState.StopGuess
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="ESL职业联赛欧洲区小组赛",
                    MatchTimeUtc=DateTime.Parse("2017-04-02 1:00")
                },
                new MatchListItem
                {
                    MasterTeam ="HR",
                    MasterTeamIcon ="/images/game_teams/3.png",
                    MasterTeamScore=0,
                    MasterTeamRate=1.23m,

                    SlaveTeam="EnVyUs",
                    SlaveTeamIcon="/images/game_teams/4.png",
                    SlaveTeamScore=0,
                    SlaveTeamRate=1.55m,

                    GameIcon="/images/games/cs.png",
                    MatchName="ESL职业联赛欧洲区小组赛",
                    MatchTimeUtc=DateTime.Parse("2017-04-07 2:00"),

                    MatchState= MatchState.StopGuess
                }
            };

            return View(models);
        }
    }
}