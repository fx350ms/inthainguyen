using Abp.Application.Services.Dto;
using InTN.Controllers;
using InTN.Customers;
using InTN.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InTN.Web.Controllers
{
    public class CustomersController : InTNControllerBase
    {
        private readonly ICustomerAppService _customerService;

        public CustomersController(ICustomerAppService customerAppService,
          IUserAppService userService)

        {
            _customerService = customerAppService;
        }


        public async Task<IActionResult> Index()
        {
            return View();
        }


        public async Task<ActionResult> EditModal(int id)
        {
            var customer = await _customerService.GetAsync(new EntityDto(id));
            return PartialView("_EditModal", customer);
        }

        //public async Task<IActionResult> Create()
        //{
        //    return View();
        //}
    }
}
