using Game.Base;
using Game.Web.Models.Matches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Web.Factories
{
    public interface IMatchModelFactory
    {
        /// <summary>
        /// 查找比赛
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="keyword">查找关键词</param>
        /// <param name="id">比赛Id</param>
        /// <returns></returns>
        IPagedList<MatchListItem> PrepareMatchModel(
            int gameId, 
            int pageSize, 
            int pageIndex, 
            string keyword, 
            int id);
    }
}
