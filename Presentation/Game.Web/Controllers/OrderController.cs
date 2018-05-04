using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Base;
using Game.Facade.Mvc.Filters;
using Game.Facade.Security;
using Game.Services.Orders;
using Game.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace Game.Web.Controllers
{
    [AuthorizeRegisted]
    [HttpsRequirement(SslRequirement.Yes)]
    public class OrderController : BasePublicController
    {
        private readonly IOrderService _orderService;
        private readonly IWorkContext _workContext;
        private readonly IOrderModelFactory _orderModelFactory;

        public OrderController(IOrderService orderService,
            IOrderModelFactory orderModelFactory)
        {
            this._orderService = orderService;
            this._orderModelFactory = orderModelFactory;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public IActionResult List()
        {
            var model = _orderModelFactory.PrepareOrderDetailsModel();

            return View(model);
        }
    }
}