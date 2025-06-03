using Abp.Application.Services.Dto;
using InTN.Controllers;
using InTN.ProductProperties;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InTN.Web.Controllers
{
    public class ProductPropertiesController : InTNControllerBase
    {
        private readonly IProductPropertyAppService _productPropertyService;

        public ProductPropertiesController(IProductPropertyAppService productPropertyService)
        {
            _productPropertyService = productPropertyService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> EditModal(int id)
        {
            var productProperty = await _productPropertyService.GetAsync(new EntityDto(id));
            return PartialView("_EditModal", productProperty);
        }
    }
}