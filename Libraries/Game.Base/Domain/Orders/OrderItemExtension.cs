using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Base.Domain.Orders
{
    public static class OrderItemExtension
    {
        public static decimal GuessRate(this OrderItem @this)
        {
            if (@this == null)
                throw new ArgumentNullException();

            if (@this.Match == null)
                throw new InvalidOperationException("订单项比赛不能为空");

            if (@this.Match.MasterTeamId == 0 || @this.Match.SlaveTeamId == 0 || @this.TeamId == 0)
                throw new InvalidOperationException("此订单项没有竞猜，或不存在比赛团队");

            if (@this.Match.MasterTeamId == @this.TeamId)
                return @this.Match.MasterTeamRate;

            if (@this.Match.SlaveTeamId == @this.TeamId)
                return @this.Match.SlaverTeamRate;

            throw new InvalidOperationException("竞猜团队不存在于比赛团队中");
        }
    }
}
