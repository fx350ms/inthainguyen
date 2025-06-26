using InTN.ProductNotes.Dto;
using System.Collections.Generic;

namespace InTN.Web.Models.ProductNotes
{
    public class CreateProductNoteModel
    {
        public List<ProductNoteDto> ParentNotes { get; set; } = new List<ProductNoteDto>();
        public ProductNoteDto ProductNote { get; set; } = new ProductNoteDto();
        
    }
}
