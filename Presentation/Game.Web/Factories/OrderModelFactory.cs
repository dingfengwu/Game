using Game.Base;
using Game.Base.Domain.Orders;
using Game.Services.Catalog;
using Game.Services.Directory;
using Game.Services.Helpers;
using Game.Services.Localization;
using Game.Services.Orders;
using Game.Services.Payments;
using Game.Services.Seo;
using Game.Web.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Web.Factories
{
    /// <summary>
    /// Represents the order model factory
    /// </summary>
    public partial class OrderModelFactory : IOrderModelFactory
    {
        #region Fields
        
        private readonly IOrderService _orderService;
        private readonly IWorkContext _workContext;
        private readonly ICurrencyService _currencyService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IPaymentService _paymentService;
        private readonly ILocalizationService _localizationService;
        private readonly IPriceFormatter _priceFormatter;

        #endregion

        #region Ctor

        public OrderModelFactory(IOrderService orderService,
            IWorkContext workContext,
            ICurrencyService currencyService,
            IDateTimeHelper dateTimeHelper,
            IPaymentService paymentService,
            IPriceFormatter priceFormatter,
            ILocalizationService localizationService)
        {
            this._orderService = orderService;
            this._workContext = workContext;
            this._currencyService = currencyService;
            this._dateTimeHelper = dateTimeHelper;
            this._paymentService = paymentService;
            this._localizationService = localizationService;
            this._priceFormatter = priceFormatter;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare the customer order list model
        /// </summary>
        /// <returns>Customer order list model</returns>
        public virtual CustomerOrderListModel PrepareCustomerOrderListModel()
        {
            var model = new CustomerOrderListModel();
            var orders = _orderService.SearchOrders(customerId: _workContext.CurrentCustomer.Id);
            foreach (var order in orders)
            {
                var orderModel = new CustomerOrderListModel.OrderDetailsModel
                {
                    Id = order.Id,
                    CreatedOn = _dateTimeHelper.ConvertToUserTime(order.CreatedOnUtc, DateTimeKind.Utc),
                    OrderStatusEnum = order.OrderStatus,
                    OrderStatus = order.OrderStatus.GetLocalizedEnum(_localizationService, _workContext),
                    PaymentStatus = order.PaymentStatus.GetLocalizedEnum(_localizationService, _workContext),
                    CustomOrderNumber = order.CustomOrderNumber
                };
                var orderTotalInCustomerCurrency = _currencyService.ConvertCurrency(order.OrderTotal, order.CurrencyRate);
                orderModel.OrderTotal = _priceFormatter.FormatPrice(orderTotalInCustomerCurrency, true, order.CustomerCurrencyCode, _workContext.WorkingLanguage);

                model.Orders.Add(orderModel);
            }

            return model;
        }

        /// <summary>
        /// Prepare the order details model
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Order details model</returns>
        public virtual OrderDetailsModel PrepareOrderDetailsModel(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            var model = new OrderDetailsModel
            {
                Id = order.Id,
                CreatedOn = _dateTimeHelper.ConvertToUserTime(order.CreatedOnUtc, DateTimeKind.Utc),
                OrderStatus = order.OrderStatus.GetLocalizedEnum(_localizationService, _workContext)
            };
            
            //total
            var orderTotalInCustomerCurrency = _currencyService.ConvertCurrency(order.OrderTotal, order.CurrencyRate);
            model.OrderTotal = _priceFormatter.FormatPrice(orderTotalInCustomerCurrency, true, order.CustomerCurrencyCode, _workContext.WorkingLanguage);
            
            //purchased products
            var orderItems = order.OrderItems;
            foreach (var orderItem in orderItems)
            {
                var orderItemModel = new OrderDetailsModel.OrderItemModel
                {
                    Id = orderItem.Id,
                    OrderItemGuid = orderItem.OrderItemGuid,
                    Quantity = orderItem.Quantity,
                };
                model.Items.Add(orderItemModel);
            }

            return model;
        }

        /// <summary>
        /// Prepare the order details model
        /// </summary>
        /// <returns>Order details model</returns>
        public virtual IPagedList<OrderDetailsModel.OrderItemModel> PrepareOrderDetailsModel()
        {
            //TODO:需要优化
            var list = new List<OrderDetailsModel.OrderItemModel>();
            var orderItems = _orderService.SearchOrderItems(customerId: _workContext.CurrentCustomer.Id);
            var models = new PagedList<OrderDetailsModel.OrderItemModel>(list, orderItems.PageIndex, orderItems.PageSize, orderItems.TotalCount);
            foreach (var orderItem in orderItems)
            {
                var orderItemModel = new OrderDetailsModel.OrderItemModel
                {
                    Id = orderItem.Id,
                    OrderId=orderItem.OrderId,
                    CustomerOrderNo=orderItem.Order.CustomOrderNumber,
                    OrderItemGuid = orderItem.OrderItemGuid,
                    Quantity = orderItem.Quantity,
                    GameId = orderItem.Match.GameId,
                    GameName = orderItem.Match.Game.Name,
                    GameIcon = orderItem.Match.Game.Icon,
                    MatchId = orderItem.MatchId,
                    MatchName = orderItem.Match.MatchName,
                    MatchTimeLocal = _dateTimeHelper.ConvertToUserTime(orderItem.Match.MatchTimeUtc, DateTimeKind.Utc),
                    MasterTeamId = orderItem.Match.MasterTeamId,
                    MasterTeamName = orderItem.Match.MasterTeam.Name,
                    MasterRate = orderItem.Match.MasterTeamRate,
                    MasterScore = orderItem.Match.MasterTeamScore,
                    SlaverTeamId = orderItem.Match.SlaveTeamId,
                    SlaverTeamName = orderItem.Match.SlaverTeam.Name,
                    SlaverTeamRate = orderItem.Match.SlaverTeamRate,
                    SlaverTeamScore = orderItem.Match.SlaverTeamScore,
                    TeamId = orderItem.TeamId,
                    TeamName = orderItem.Team.Name,
                    UnitPrice = orderItem.UnitPrice
                };

                var orderItemAmount = _currencyService.ConvertCurrency(orderItem.Quantity * orderItem.UnitPrice, orderItem.Order.CurrencyRate);
                orderItemModel.SubTotal = _priceFormatter.FormatPrice(orderItemAmount, true, orderItem.Order.CustomerCurrencyCode, _workContext.WorkingLanguage);

                var reward = orderItem.Quantity * orderItem.UnitPrice + orderItem.Quantity * orderItem.GuessRate();
                var orderItemWishReward = _currencyService.ConvertCurrency(reward, orderItem.Order.CurrencyRate);
                orderItemModel.WishReward = _priceFormatter.FormatPrice(orderItemWishReward, true, orderItem.Order.CustomerCurrencyCode, _workContext.WorkingLanguage);

                models.Add(orderItemModel);
            }
            return models;
        }
        #endregion
    }
}
