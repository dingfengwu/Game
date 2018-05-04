using System;
using System.Collections.Generic;
using Game.Facade.Mvc.Models;
using Game.Web.Models.Common;

namespace Game.Web.Models.Order
{
    public partial class OrderDetailsModel : BaseGameEntityModel
    {
        public OrderDetailsModel()
        {
            Items = new List<OrderItemModel>();
        }

        public string CustomOrderNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public string OrderStatus { get; set; }
        public string OrderTotal { get; set; }
        
        public IList<OrderItemModel> Items { get; set; }
        
		#region Nested Classes

        public partial class OrderItemModel : BaseGameEntityModel
        {
            public int OrderId { get; set; }
            public string CustomerOrderNo { get; set; }
            public Guid OrderItemGuid { get; set; }
            public int MatchId { get; set; }
            public string MatchName { get; set; }
            public DateTime MatchTimeLocal { get; set; }
            public int MasterTeamId { get; set; }
            public string MasterTeamName { get; set; }
            public decimal MasterScore { get; set; }
            public decimal MasterRate { get; set; }
            public int SlaverTeamId { get; set; }
            public string SlaverTeamName { get; set; }
            public decimal SlaverTeamScore { get; set; }
            public decimal SlaverTeamRate { get; set; }
            public int GameId { get; set; }
            public string GameName { get; set; }
            public string GameIcon { get; set; }
            /// <summary>
            /// 我竞猜会赢的团队Id
            /// </summary>
            public int TeamId { get; set; }
            /// <summary>
            /// 我竞猜会赢的团队名称
            /// </summary>
            public string TeamName { get; set; }
            /// <summary>
            /// 每注单价
            /// </summary>
            public decimal UnitPrice { get; set; }
            /// <summary>
            /// 小计
            /// </summary>
            public string SubTotal { get; set; }
            /// <summary>
            /// 倍数
            /// </summary>
            public int Quantity { get; set; }
            /// <summary>
            /// 预期奖金
            /// </summary>
            public string WishReward { get; set; }
        }
		#endregion
    }
}