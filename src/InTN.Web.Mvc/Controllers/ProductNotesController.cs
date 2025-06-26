using Abp.Application.Services.Dto;
using InTN.Controllers;
using InTN.ProductNotes;
using InTN.Products.Dto;
using InTN.Web.Models.ProductNotes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InTN.Web.Controllers
{
    public class ProductNotesController : InTNControllerBase
    {
        private readonly IProductNoteAppService _productNoteAppService;

        public ProductNotesController(IProductNoteAppService productNoteAppService)
        {
            _productNoteAppService = productNoteAppService;
        }

        public async Task<IActionResult> Index()
        {
            //var notes = await _productNoteAppService.GetNotesByProductIdAsync(productId);
            //return View(notes);
            return View();
        }

        public async Task<ActionResult> EditModal(int id)
        {
            var note = await _productNoteAppService.GetAsync(new EntityDto(id));
            return PartialView("_EditModal", note);
        }

        public async Task<ActionResult> CreateModal(int productId)
        {
            var model = new CreateProductNoteModel { 

                };
            return PartialView("_CreateModal", model);
        }
    }
}