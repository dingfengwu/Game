using System;
using System.Collections.Generic;
using Game.Base.Domain.Orders;
using Game.Facade.Mvc.Models;

namespace Game.Web.Models.Order
{
    public partial class CustomerOrderListModel : BaseGameModel
    {
        public CustomerOrderListModel()
        {
            Orders = new List<OrderDetailsModel>();
        }

        public IList<OrderDetailsModel> Orders { get; set; }

        #region Nested classes

        public partial class OrderDetailsModel : BaseGameEntityModel
        {
            public string CustomOrderNumber { get; set; }
            public string OrderTotal { get; set; }
            public OrderStatus OrderStatusEnum { get; set; }
            public string OrderStatus { get; set; }
            public string PaymentStatus { get; set; }
            public DateTime CreatedOn { get; set; }
        }

        #endregion
    }
}