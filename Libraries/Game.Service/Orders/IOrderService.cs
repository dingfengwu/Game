using System;
using System.Collections.Generic;
using Game.Base;
using Game.Base.Domain.Orders;

namespace Game.Services.Orders
{
    /// <summary>
    /// Order service interface
    /// </summary>
    public partial interface IOrderService
    {
        #region Orders

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="orderId">The order identifier</param>
        /// <returns>Order</returns>
        Order GetOrderById(int orderId);

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="customOrderNumber">The custom order number</param>
        /// <returns>Order</returns>
        Order GetOrderByCustomOrderNumber(string customOrderNumber);

        /// <summary>
        /// Get orders by identifiers
        /// </summary>
        /// <param name="orderIds">Order identifiers</param>
        /// <returns>Order</returns>
        IList<Order> GetOrdersByIds(int[] orderIds);

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="orderGuid">The order identifier</param>
        /// <returns>Order</returns>
        Order GetOrderByGuid(Guid orderGuid);

        /// <summary>
        /// Deletes an order
        /// </summary>
        /// <param name="order">The order</param>
        void DeleteOrder(Order order);

        /// <summary>
        /// Search orders
        /// </summary>
        /// <param name="customerId">Customer identifier; null to load all orders</param>
        /// <param name="matchId">Product identifier which was purchased in an order; 0 to load all orders</param>
        /// <param name="paymentMethodSystemName">Payment method system name; null to load all records</param>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="osIds">Order status identifiers; null to load all orders</param>
        /// <param name="psIds">Payment status identifiers; null to load all orders</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Orders</returns>
        IPagedList<Order> SearchOrders(int customerId = 0,
            int matchId = 0,string paymentMethodSystemName = null,
            DateTime? createdFromUtc = null, DateTime? createdToUtc = null,
            List<int> osIds = null, List<int> psIds = null,
            int pageIndex = 0, int pageSize = int.MaxValue);
        
        /// <summary>
        /// Inserts an order
        /// </summary>
        /// <param name="order">Order</param>
        void InsertOrder(Order order);

        /// <summary>
        /// Updates the order
        /// </summary>
        /// <param name="order">The order</param>
        void UpdateOrder(Order order);

        /// <summary>
        /// Get an order by authorization transaction ID and payment method system name
        /// </summary>
        /// <param name="authorizationTransactionId">Authorization transaction ID</param>
        /// <param name="paymentMethodSystemName">Payment method system name</param>
        /// <returns>Order</returns>
        Order GetOrderByAuthorizationTransactionIdAndPaymentMethod(string authorizationTransactionId, string paymentMethodSystemName);

        #endregion

        #region Orders items

        /// <summary>
        /// Gets an order item
        /// </summary>
        /// <param name="orderItemId">Order item identifier</param>
        /// <returns>Order item</returns>
        OrderItem GetOrderItemById(int orderItemId);

        /// <summary>
        /// Gets an order item
        /// </summary>
        /// <param name="orderItemGuid">Order item identifier</param>
        /// <returns>Order item</returns>
        OrderItem GetOrderItemByGuid(Guid orderItemGuid);

        /// <summary>
        /// Delete an order item
        /// </summary>
        /// <param name="orderItem">The order item</param>
        void DeleteOrderItem(OrderItem orderItem);

        /// <summary>
        /// 查询订单项
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="orderId">订单Id</param>
        /// <param name="customerOrderNo">订单号</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        IPagedList<OrderItem> SearchOrderItems(int customerId = 0, int orderId = 0,
            string customerOrderNo = "", int pageIndex = 0, int pageSize = int.MaxValue);

        #endregion

    }
}
