using Game.Base.Domain.Matches;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Services.Matches
{
    public interface IMatchService
    {
        /// <summary>
        /// 新增比赛
        /// </summary>
        /// <param name="model"></param>
        void InsertMatch(Match model);

        /// <summary>
        /// 修改比赛
        /// </summary>
        /// <param name="model"></param>
        void UpdateMatch(Match model);

        /// <summary>
        /// 获取比赛
        /// </summary>
        /// <param name="model"></param>
        Match GetMatch(int id);

        /// <summary>
        /// 获取可见的游戏
        /// </summary>
        /// <returns></returns>
        List<MatchGame> GetAvailableGames();

        /// <summary>
        /// 获取可见的团队
        /// </summary>
        /// <returns></returns>
        List<MatchTeam> GetAvailableTeams();

        /// <summary>
        /// 获取所有可用的比赛
        /// </summary>
        /// <returns></returns>
        List<MatchCacheModel> GetAvailableMatch();

        /// <summary>
        /// 创建缓存模型
        /// </summary>
        /// <param name="match">比赛</param>
        /// <returns></returns>
        MatchCacheModel CreateMatchCacheModel(Match match);
    }
}
