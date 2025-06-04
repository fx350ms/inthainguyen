using Abp.Application.Services.Dto;
using InTN.Brands;
using InTN.Controllers;
using InTN.ProductCategories;
using InTN.Products;
using InTN.Products.Dto;
using InTN.ProductTypes;
using InTN.Suppliers;
using InTN.Web.Models.Products;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace InTN.Web.Controllers
{
    public class ProductsController : InTNControllerBase
    {
        private readonly IProductAppService _productService;
        private readonly IProductCategoryAppService _productCategoryService;
        private readonly IProductTypeAppService _productTypeService;
        private readonly ISupplierAppService _supplierService;
        private readonly IBrandAppService _brandService;


        public ProductsController(IProductAppService productService,
            IProductCategoryAppService productCategoryService,
            IProductTypeAppService productTypeService,
            ISupplierAppService supplierService,
            IBrandAppService brandService
        )
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _productTypeService = productTypeService;
            _supplierService = supplierService;
            _brandService = brandService;

        }

        public async Task<IActionResult> Index()
        {

            var model = new ProductIndexModel()
            {
                Brands = (await _brandService.GetAllAsync(new PagedResultRequestDto())).Items.ToList(),
                ProductCategories = (await _productCategoryService.GetAllAsync(new PagedResultRequestDto())).Items.ToList(),
                ProductTypes = (await _productTypeService.GetAllAsync(new PagedResultRequestDto())).Items.ToList(),
                Suppliers = (await _supplierService.GetAllAsync(new PagedResultRequestDto())).Items.ToList()
            };
            return View(model);
        }

        public async Task<ActionResult> EditModal(int id)
        {
            var product = await _productService.GetAsync(new EntityDto(id));
            return PartialView("_EditModal", product);
        }

        public async Task<ActionResult> CreateModal()
        {
            return PartialView("_CreateModal", new ProductCreateModel()
            {
                Brands = (await _brandService.GetAllAsync(new PagedResultRequestDto())).Items.ToList(),
                ProductCategories = (await _productCategoryService.GetAllAsync(new PagedResultRequestDto())).Items.ToList(),
                ProductTypes = (await _productTypeService.GetAllAsync(new PagedResultRequestDto())).Items.ToList(),
                Suppliers = (await _supplierService.GetAllAsync(new PagedResultRequestDto())).Items.ToList(),
                Dto = new CreateProductDto()

            });
        }
    }
}