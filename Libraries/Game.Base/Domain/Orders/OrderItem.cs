using Game.Base.Domain.Matches;
using System;

namespace Game.Base.Domain.Orders
{
    /// <summary>
    /// Represents an order item
    /// </summary>
    public partial class OrderItem : BaseEntity
    {
        #region 基本属性

        /// <summary>
        /// Gets or sets the order item identifier
        /// </summary>
        public Guid OrderItemGuid { get; set; }

        /// <summary>
        /// Gets or sets the order identifier
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 玩法Id
        /// </summary>
        public int PlayId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int MatchId { get; set; }

        /// <summary>
        /// 团队Id
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        #endregion

        #region 导航属性

        /// <summary>
        /// Gets the order
        /// </summary>
        public virtual Order Order { get; set; }

        /// <summary>
        /// Gets the product
        /// </summary>
        public virtual Match Match { get; set; }

        /// <summary>
        /// 竞猜会赢的团队
        /// </summary>
        public virtual MatchTeam Team { get; set; }

        #endregion
    }
}
