using Abp.Application.Services.Dto;
using InTN.Controllers;
using InTN.Customers;
using InTN.IdentityCodes;
using InTN.Orders;
using InTN.Orders.Dto;
using InTN.Transactions;
using InTN.Transactions.Dto;
using InTN.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InTN.Web.Controllers
{
    public class TransactionController : InTNControllerBase
    {
        private readonly IIdentityCodeAppService _identityCodeAppService;
        public TransactionController( 
             IIdentityCodeAppService identityCodeAppService
         )
        {
            _identityCodeAppService = identityCodeAppService;
        }


        public async Task<IActionResult> Index()
        {
            return View();
        }


        public async Task<IActionResult> Create()
        {
         
            var identityCode = await _identityCodeAppService.GenerateNewSequentialNumberAsync("GD");
            var model = new TransactionDto
            {
                TransactionCode = identityCode.Code,
            };

            return View(model);
        }

    }
}
