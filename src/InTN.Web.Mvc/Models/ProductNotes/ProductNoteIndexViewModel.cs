using InTN.ProductCategories.Dto;
using InTN.ProductNotes.Dto;
using System.Collections.Generic;

namespace InTN.Web.Models.ProductNotes
{
    public class ProductNoteIndexViewModel
    {
        public List<ProductCategoryDto> ProductCategories { get; set; } = new List<ProductCategoryDto>();
    }

    public class ProductNoteCreateOrEditViewModel
    {
        public List<ProductCategoryDto> ProductCategories { get; set; } = new List<ProductCategoryDto>();
        public ProductNoteDto ProductNote { get; set; } = new ProductNoteDto();
    }
}
