using Abp.Application.Services.Dto;
using InTN.Controllers;
using InTN.ProductCategories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace InTN.Web.Controllers
{
    public class ProductCategoriesController : InTNControllerBase
    {
        private readonly IProductCategoryAppService _productCategoryService;

        public ProductCategoriesController(IProductCategoryAppService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        public async Task<IActionResult> Index()
        {
            // var productCategories = await _productCategoryService.GetAllAsync(new PagedResultRequestDto());
            // ViewBag.ProductCategories = productCategories.Items.ToList();
              var productCategories = await _productCategoryService.GetAllAsync(new PagedResultRequestDto());
            ViewBag.ParentCategories = productCategories.Items.ToList(); // Truyền danh sách danh 
            return View();
           
        }

        public async Task<ActionResult> EditModal(int id)
        {
            var productCategory = await _productCategoryService.GetAsync(new EntityDto(id));
            return PartialView("_EditModal", productCategory);
        }
    }
}