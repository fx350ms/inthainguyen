using Abp.Application.Services.Dto;

namespace InTN.ProductNotes.Dto
{
    public class ProductNoteDto : EntityDto<int>
    {
        public int? ParentId { get; set; }
        public int ProductId { get; set; }
        public string Note { get; set; } = string.Empty; // Nội dung ghi chú
        public string ParentNote { get; set; } = string.Empty; // Nội dung ghi chú cha (nếu có)
    }
}