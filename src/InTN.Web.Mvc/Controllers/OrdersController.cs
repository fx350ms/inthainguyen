using Abp.Application.Services.Dto;
using InTN.Controllers;
using InTN.Customers;
using InTN.IdentityCodes;
using InTN.OrderLogs;
using InTN.Orders;
using InTN.Orders.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InTN.Web.Controllers
{
    public class OrdersController : InTNControllerBase
    {
        private readonly IIdentityCodeAppService _identityCodeAppService;
        private readonly IOrderAppService _orderAppService;
        public OrdersController(IOrderAppService orderService,
             IIdentityCodeAppService identityCodeAppService
         )
        {
            _orderAppService = orderService;
            _identityCodeAppService = identityCodeAppService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var identityCode = await _identityCodeAppService.GenerateNewSequentialNumberAsync("DH");
            var model = new CreateOrderDto
            {
                OrderCode = identityCode.Code,

            };
            return View(model);
        }


        public async Task<IActionResult> CreateQuotation(int id)
        {
            var order = await _orderAppService.GetAsync(new EntityDto(id));
            if (order == null)
            {
                return NotFound();
            }
            var model = new OrderQuotationUploadDto()
            {
                OrderId = order.Id,
                OrderCode = order.OrderCode,
                 
            };
            return View(model);
        }



        public async Task<IActionResult> CreateDesign(int id)
        {
            var order = await _orderAppService.GetAsync(new EntityDto(id));
            if (order == null)
            {
                return NotFound();
            }
            var model = new OrderDesignUploadDto()
            {
                OrderId = order.Id,
                OrderCode = order.OrderCode,
            };
            return View(model);
        }


        public async Task<IActionResult> CreateDeposit(int id)
        {
            var order = await _orderAppService.GetAsync(new EntityDto(id));
            if (order == null)
            {
                return NotFound();
            }
            var model = new OrderDepositUploadDto()
            {
                OrderId = order.Id,
                OrderCode = order.OrderCode,
                TotalAmount = order.TotalAmount ?? 0
            };
            return View(model);
        }


        public async Task<IActionResult> CreateTransaction(int id)
        {
            var order = await _orderAppService.GetAsync(new EntityDto(id));
            if (order == null)
            {
                return NotFound();
            }
            var model = new OrderDepositUploadDto()
            {
                OrderId = order.Id,
                OrderCode = order.OrderCode,
                TotalAmount = order.TotalAmount ?? 0
            };
            return View(model);
        }

        
    }
}
