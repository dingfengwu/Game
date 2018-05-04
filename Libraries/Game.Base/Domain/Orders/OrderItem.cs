using Game.Base.Domain.Matches;
using System;

namespace Game.Base.Domain.Orders
{
    /// <summary>
    /// Represents an order item
    /// </summary>
    public partial class OrderItem : BaseEntity
    {
        #region ��������

        /// <summary>
        /// Gets or sets the order item identifier
        /// </summary>
        public Guid OrderItemGuid { get; set; }

        /// <summary>
        /// Gets or sets the order identifier
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// �淨Id
        /// </summary>
        public int PlayId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int MatchId { get; set; }

        /// <summary>
        /// �Ŷ�Id
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public decimal UnitPrice { get; set; }

        #endregion

        #region ��������

        /// <summary>
        /// Gets the order
        /// </summary>
        public virtual Order Order { get; set; }

        /// <summary>
        /// Gets the product
        /// </summary>
        public virtual Match Match { get; set; }

        /// <summary>
        /// ���»�Ӯ���Ŷ�
        /// </summary>
        public virtual MatchTeam Team { get; set; }

        #endregion
    }
}
