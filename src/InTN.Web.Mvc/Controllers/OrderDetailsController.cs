using Abp.Application.Services.Dto;
using InTN.Controllers;
using InTN.Orders;
using InTN.Orders.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InTN.Web.Controllers
{
    public class OrderDetailsController : InTNControllerBase
    {
        private readonly IOrderDetailAppService _orderDetailAppService;

        public OrderDetailsController(IOrderDetailAppService orderDetailAppService)
        {
            _orderDetailAppService = orderDetailAppService;
        }

        public async Task<IActionResult> Index(int orderId)
        {
            var orderDetails = await _orderDetailAppService.GetOrderDetailsByOrderIdAsync(orderId);
            return View(orderDetails);
        }

        public async Task<ActionResult> EditModal(int id)
        {
            var orderDetail = await _orderDetailAppService.GetAsync(new EntityDto(id));
            return PartialView("_EditModal", orderDetail);
        }

        public async Task<ActionResult> CreateModal(int orderId)
        {
            var model = new OrderDetailDto { OrderId = orderId };
            return PartialView("_CreateModal", model);
        }
    }
}