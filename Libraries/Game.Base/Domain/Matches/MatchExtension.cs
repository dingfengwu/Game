using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Base.Domain.Matches
{
    public static class MatchExtension
    {
        public static MatchState GetMatchState(this Match @this)
        {
            //TODO:最好将此方法移入MatchService中，且需要根据配置来决定比赛的当前状态

            if (@this == null)
                throw new ArgumentNullException();

            if (@this.Id == 0)
                return MatchState.None;

            if (!@this.Enabled)
                return MatchState.None;

            if (@this.Enabled && @this.MatchTimeUtc <= DateTime.UtcNow)
                return MatchState.StopGuess;

            return MatchState.Guessing;
        }
    }
}
