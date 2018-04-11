using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Base.Domain.Matches
{
    /// <summary>
    /// 竞猜状态
    /// </summary>
    public enum MatchState
    {
        /// <summary>
        /// 暂无竞猜
        /// </summary>
        None=0,
        /// <summary>
        /// 竞猜中
        /// </summary>
        Guessing=1,
        /// <summary>
        /// 停止竞猜
        /// </summary>
        StopGuess=2
    }
}
