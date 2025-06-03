using Abp.Application.Services.Dto;
using InTN.Controllers;
using InTN.ProductTypes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InTN.Web.Controllers
{
    public class ProductTypesController : InTNControllerBase
    {
        private readonly IProductTypeAppService _productTypeService;

        public ProductTypesController(IProductTypeAppService productTypeService)
        {
            _productTypeService = productTypeService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> EditModal(int id)
        {
            var productType = await _productTypeService.GetAsync(new EntityDto(id));
            return PartialView("_EditModal", productType);
        }
    }
}