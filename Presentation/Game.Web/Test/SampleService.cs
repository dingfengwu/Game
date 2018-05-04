using Game.Base.Caching;
using Game.Base.Domain.Matches;
using Game.Base.Infrastructure;
using Game.Services.Events;
using Game.Services.Matches;
using Game.Web.Models.Matches;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Web.Test
{
    public static class SampleService
    {
        public static void SetAllTestData()
        {
            var cacheManager = EngineContext.Current.Resolve<IStaticCacheManager>();
            if (!cacheManager.IsSet(ModelCacheEventConsumer.AVAILABLE_MATCH_GAME_LIST))
                SetMatchGameCache();
            if (!cacheManager.IsSet(ModelCacheEventConsumer.AVAILABLE_MATCH_TEAM_LIST))
                SetMatchTeamCache();
            if (!cacheManager.IsSet(ModelCacheEventConsumer.AVAILABLE_MATCH_LIST))
                SetMatchCache();
        }

        public static void SetMatchTeamCache()
        {
            var cacheManager = EngineContext.Current.Resolve<IStaticCacheManager>();
            cacheManager.Set(ModelCacheEventConsumer.AVAILABLE_MATCH_TEAM_LIST, new List<MatchTeam>()
            {
                new MatchTeam{
                    Id=new Random().Next(1,1000000),
                    Name="测试团一",
                    Icon="1.png",
                    CreateTimeUtc=DateTime.UtcNow,
                    CreateUserId=1,
                    Enabled=true
                },
                new MatchTeam{
                    Id=new Random().Next(1,1000000),
                    Name="测试团二",
                    Icon="2.png",
                    CreateTimeUtc=DateTime.UtcNow,
                    CreateUserId=1,
                    Enabled=true
                },
                new MatchTeam{
                    Id=new Random().Next(1,1000000),
                    Name="测试团三",
                    Icon="3.png",
                    CreateTimeUtc=DateTime.UtcNow,
                    CreateUserId=1,
                    Enabled=true
                },
                new MatchTeam{
                    Id=new Random().Next(1,1000000),
                    Name="测试团四",
                    Icon="4.png",
                    CreateTimeUtc=DateTime.UtcNow,
                    CreateUserId=1,
                    Enabled=true
                }
            }, 1 * 24 * 60);
        }

        public static void SetMatchGameCache()
        {
            var cacheManager = EngineContext.Current.Resolve<IStaticCacheManager>();
            cacheManager.Set(ModelCacheEventConsumer.AVAILABLE_MATCH_GAME_LIST, new List<MatchGame>()
            {
                new MatchGame{
                    Id=new Random().Next(1,1000000),
                    Name="游戏CS",
                    Icon="cs.png",
                    CreateTimeUtc=DateTime.UtcNow,
                    CreateUserId=1,
                    Enabled=true
                },
                new MatchGame{
                    Id=new Random().Next(1,1000000),
                    Name="游戏DOTAII",
                    Icon="dota2.png",
                    CreateTimeUtc=DateTime.UtcNow,
                    CreateUserId=1,
                    Enabled=true
                },
                new MatchGame{
                    Id=new Random().Next(1,1000000),
                    Name="游戏LOL",
                    Icon="lol.png",
                    CreateTimeUtc=DateTime.UtcNow,
                    CreateUserId=1,
                    Enabled=true
                },
                new MatchGame{
                    Id=new Random().Next(1,1000000),
                    Name="游戏炉石传说",
                    Icon="lsqs.png",
                    CreateTimeUtc=DateTime.UtcNow,
                    CreateUserId=1,
                    Enabled=true
                },
                new MatchGame{
                    Id=new Random().Next(1,1000000),
                    Name="游戏王者",
                    Icon="wzry.png",
                    CreateTimeUtc=DateTime.UtcNow,
                    CreateUserId=1,
                    Enabled=true
                }
            }, 1 * 24 * 60);
        }

        public static void SetMatchCache()
        {
            var rnd = new Random();
            var matchService = EngineContext.Current.Resolve<IMatchService>();
            var allGame = matchService.GetAvailableGames();
            var allTeam = matchService.GetAvailableTeams();
            var matchCacheModel = new List<MatchCacheModel>();
            for (var i = 0; i < 50; i++)
            {
                var gameIndex = rnd.Next(0, allGame.Count - 1);
                var teamIndex = rnd.Next(0, allTeam.Count - 1);
                var teamIndex1= rnd.Next(0, allTeam.Count - 1);
                matchCacheModel.Add(new MatchCacheModel
                {
                    CreateTimeLocal=DateTime.UtcNow,
                    CreateUserId=1,
                    Enabled=true,
                    GameIcon= allGame[gameIndex].Icon,
                    GameId=allGame[gameIndex].Id,
                    LiveUrl="http://www.baidu.com;http://www.google.com",
                    MasterTeam= allTeam[teamIndex].Name,
                    MasterTeamIcon=allTeam[teamIndex].Icon,
                    MasterTeamId=allTeam[teamIndex].Id,
                    MasterTeamRate=1.76m*rnd.Next(1,5),
                    MasterTeamScore=rnd.Next(1,10),
                    MatchId=rnd.Next(1,1000),
                    MatchMemo="",
                    MatchName="测试比赛"+i.ToString(),
                    MatchState= MatchState.Guessing,
                    MatchTimeLocal=DateTime.UtcNow.AddDays(10),
                    SlaveTeam=allGame[teamIndex1].Name,
                    SlaveTeamIcon= allTeam[teamIndex1].Icon,
                    SlaveTeamId=allGame[teamIndex1].Id,
                    SlaveTeamRate=0.09m*rnd.Next(2,9),
                    SlaveTeamScore=rnd.Next(1,100)
                });
            }


            var cacheManager = EngineContext.Current.Resolve<IStaticCacheManager>();
            cacheManager.Set(ModelCacheEventConsumer.AVAILABLE_MATCH_LIST, matchCacheModel, 1 * 24 * 60);
            
        }
    }
}
