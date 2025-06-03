using Abp.Application.Services.Dto;
using InTN.Controllers;
using InTN.Brands;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InTN.Web.Controllers
{
    public class BrandsController : InTNControllerBase
    {
        private readonly IBrandAppService _brandService;

        public BrandsController(IBrandAppService brandService)
        {
            _brandService = brandService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> EditModal(int id)
        {
            var brand = await _brandService.GetAsync(new EntityDto(id));
            return PartialView("_EditModal", brand);
        }
    }
}