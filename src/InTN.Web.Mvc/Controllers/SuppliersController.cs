using Abp.Application.Services.Dto;
using InTN.Controllers;
using InTN.Suppliers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InTN.Web.Controllers
{
    public class SuppliersController : InTNControllerBase
    {
        private readonly ISupplierAppService _supplierService;

        public SuppliersController(ISupplierAppService supplierService)
        {
            _supplierService = supplierService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> EditModal(int id)
        {
            var supplier = await _supplierService.GetAsync(new EntityDto(id));
            return PartialView("_EditModal", supplier);
        }
    }
}