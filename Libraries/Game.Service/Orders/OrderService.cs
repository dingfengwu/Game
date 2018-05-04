using Game.Base;
using Game.Base.Data;
using Game.Base.Domain.Customers;
using Game.Base.Domain.Matches;
using Game.Base.Domain.Orders;
using Game.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Services.Orders
{
    /// <summary>
    /// Order service
    /// </summary>
    public partial class OrderService : IOrderService
    {
        #region Fields

        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IRepository<Match> _productRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="orderRepository">Order repository</param>
        /// <param name="orderItemRepository">Order item repository</param>
        /// <param name="orderNoteRepository">Order note repository</param>
        /// <param name="productRepository">Product repository</param>
        /// <param name="recurringPaymentRepository">Recurring payment repository</param>
        /// <param name="customerRepository">Customer repository</param>
        /// <param name="eventPublisher">Event published</param>
        public OrderService(IRepository<Order> orderRepository,
            IRepository<OrderItem> orderItemRepository,
            IRepository<Match> productRepository,
            IRepository<Customer> customerRepository, 
            IEventPublisher eventPublisher)
        {
            this._orderRepository = orderRepository;
            this._orderItemRepository = orderItemRepository;
            this._productRepository = productRepository;
            this._customerRepository = customerRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        #region Orders

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="orderId">The order identifier</param>
        /// <returns>Order</returns>
        public virtual Order GetOrderById(int orderId)
        {
            if (orderId == 0)
                return null;

            return _orderRepository.GetById(orderId);
        }

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="customOrderNumber">The custom order number</param>
        /// <returns>Order</returns>
        public virtual Order GetOrderByCustomOrderNumber(string customOrderNumber)
        {
            if (string.IsNullOrEmpty(customOrderNumber))
                return null;
           
            return _orderRepository.Table.FirstOrDefault(o => o.CustomOrderNumber == customOrderNumber);
        }

        /// <summary>
        /// Get orders by identifiers
        /// </summary>
        /// <param name="orderIds">Order identifiers</param>
        /// <returns>Order</returns>
        public virtual IList<Order> GetOrdersByIds(int[] orderIds)
        {
            if (orderIds == null || orderIds.Length == 0)
                return new List<Order>();

            var query = from o in _orderRepository.Table
                        where orderIds.Contains(o.Id) && !o.Deleted
                        select o;
            var orders = query.ToList();
            //sort by passed identifiers
            var sortedOrders = new List<Order>();
            foreach (var id in orderIds)
            {
                var order = orders.Find(x => x.Id == id);
                if (order != null)
                    sortedOrders.Add(order);
            }
            return sortedOrders;
        }

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="orderGuid">The order identifier</param>
        /// <returns>Order</returns>
        public virtual Order GetOrderByGuid(Guid orderGuid)
        {
            if (orderGuid == Guid.Empty)
                return null;

            var query = from o in _orderRepository.Table
                        where o.OrderGuid == orderGuid
                        select o;
            var order = query.FirstOrDefault();
            return order;
        }

        /// <summary>
        /// Deletes an order
        /// </summary>
        /// <param name="order">The order</param>
        public virtual void DeleteOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            order.Deleted = true;
            UpdateOrder(order);

            //event notification
            _eventPublisher.EntityDeleted(order);
        }

        /// <summary>
        /// Search orders
        /// </summary>
        /// <param name="customerId">Customer identifier; 0 to load all orders</param>
        /// <param name="matchId">Product identifier which was purchased in an order; 0 to load all orders</param>
        /// <param name="paymentMethodSystemName">Payment method system name; null to load all records</param>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="osIds">Order status identifiers; null to load all orders</param>
        /// <param name="psIds">Payment status identifiers; null to load all orders</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Orders</returns>
        public virtual IPagedList<Order> SearchOrders(int customerId = 0,
            int matchId = 0,string paymentMethodSystemName = null,
            DateTime? createdFromUtc = null, DateTime? createdToUtc = null,
            List<int> osIds = null, List<int> psIds = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _orderRepository.Table;
            if (customerId > 0)
                query = query.Where(o => o.CustomerId == customerId);
            if (matchId > 0)
            {
                query = query
                    .Where(o => o.OrderItems
                    .Any(orderItem => orderItem.Match.Id == matchId));
            }
            if (!string.IsNullOrEmpty(paymentMethodSystemName))
                query = query.Where(o => o.PaymentMethodSystemName == paymentMethodSystemName);
            if (createdFromUtc.HasValue)
                query = query.Where(o => createdFromUtc.Value <= o.CreatedOnUtc);
            if (createdToUtc.HasValue)
                query = query.Where(o => createdToUtc.Value >= o.CreatedOnUtc);
            if (osIds != null && osIds.Any())
                query = query.Where(o => osIds.Contains(o.OrderStatusId));
            if (psIds != null && psIds.Any())
                query = query.Where(o => psIds.Contains(o.PaymentStatusId));
            query = query.Where(o => !o.Deleted);
            query = query.OrderByDescending(o => o.CreatedOnUtc);

            //database layer paging
            return new PagedList<Order>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Inserts an order
        /// </summary>
        /// <param name="order">Order</param>
        public virtual void InsertOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _orderRepository.Insert(order);

            //event notification
            _eventPublisher.EntityInserted(order);
        }

        /// <summary>
        /// Updates the order
        /// </summary>
        /// <param name="order">The order</param>
        public virtual void UpdateOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _orderRepository.Update(order);

            //event notification
            _eventPublisher.EntityUpdated(order);
        }

        /// <summary>
        /// Get an order by authorization transaction ID and payment method system name
        /// </summary>
        /// <param name="authorizationTransactionId">Authorization transaction ID</param>
        /// <param name="paymentMethodSystemName">Payment method system name</param>
        /// <returns>Order</returns>
        public virtual Order GetOrderByAuthorizationTransactionIdAndPaymentMethod(string authorizationTransactionId, 
            string paymentMethodSystemName)
        { 
            var query = _orderRepository.Table;
            if (!string.IsNullOrWhiteSpace(authorizationTransactionId))
                query = query.Where(o => o.AuthorizationTransactionId == authorizationTransactionId);
            
            if (!string.IsNullOrWhiteSpace(paymentMethodSystemName))
                query = query.Where(o => o.PaymentMethodSystemName == paymentMethodSystemName);
            
            query = query.OrderByDescending(o => o.CreatedOnUtc);
            var order = query.FirstOrDefault();
            return order;
        }
        
        #endregion

        #region Orders items

        /// <summary>
        /// Gets an order item
        /// </summary>
        /// <param name="orderItemId">Order item identifier</param>
        /// <returns>Order item</returns>
        public virtual OrderItem GetOrderItemById(int orderItemId)
        {
            if (orderItemId == 0)
                return null;

            return _orderItemRepository.GetById(orderItemId);
        }

        /// <summary>
        /// Gets an item
        /// </summary>
        /// <param name="orderItemGuid">Order identifier</param>
        /// <returns>Order item</returns>
        public virtual OrderItem GetOrderItemByGuid(Guid orderItemGuid)
        {
            if (orderItemGuid == Guid.Empty)
                return null;

            var query = from orderItem in _orderItemRepository.Table
                        where orderItem.OrderItemGuid == orderItemGuid
                        select orderItem;
            var item = query.FirstOrDefault();
            return item;
        }

        /// <summary>
        /// Delete an order item
        /// </summary>
        /// <param name="orderItem">The order item</param>
        public virtual void DeleteOrderItem(OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            _orderItemRepository.Delete(orderItem);

            //event notification
            _eventPublisher.EntityDeleted(orderItem);
        }

        /// <summary>
        /// 查询订单项
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <param name="orderId">订单Id</param>
        /// <param name="customerOrderNo">客户订单号</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public virtual IPagedList<OrderItem> SearchOrderItems(int customerId=0,int orderId=0,
            string customerOrderNo = "",int pageIndex=0,int pageSize=int.MaxValue)
        {
            var query = _orderItemRepository.TableNoTracking;
            if (customerId > 0)
            {
                query=query.Join(_orderRepository.TableNoTracking, x => x.OrderId, y => y.Id, (x, y) => new { Order = y, OrderItem = x })
                    .Where(p => p.Order.CustomerId == customerId)
                    .Select(p => p.OrderItem);
            }
            if (orderId > 0)
            {
                query = query.Where(p => p.OrderId == orderId);
            }
            if (!string.IsNullOrWhiteSpace(customerOrderNo))
            {
                query = query.Join(_orderRepository.TableNoTracking, x => x.OrderId, y => y.Id, (x, y) => new { Order = y, OrderItem = x })
                    .Where(p => p.Order.CustomOrderNumber == customerOrderNo)
                    .Select(p => p.OrderItem);
            }

            return new PagedList<OrderItem>(query, pageIndex, pageSize);
        }

        #endregion
        
        #endregion
    }
}
