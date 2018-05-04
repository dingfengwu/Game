using Game.Base;
using Game.Base.Domain.Orders;
using Game.Web.Models.Order;

namespace Game.Web.Factories
{
    /// <summary>
    /// Represents the interface of the order model factory
    /// </summary>
    public partial interface IOrderModelFactory
    {
        /// <summary>
        /// Prepare the customer order list model
        /// </summary>
        /// <returns>Customer order list model</returns>
        CustomerOrderListModel PrepareCustomerOrderListModel();

        /// <summary>
        /// Prepare the order details model
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Order details model</returns>
        OrderDetailsModel PrepareOrderDetailsModel(Order order);

        /// <summary>
        /// Prepare the order details model
        /// </summary>
        /// <returns>Order details model</returns>
        IPagedList<OrderDetailsModel.OrderItemModel> PrepareOrderDetailsModel();
    }
}
