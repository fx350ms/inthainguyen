using Abp.Application.Services.Dto;
using InTN.ProductCategories.Dto;

namespace InTN.ProductNotes.Dto
{
    public class ProductNoteDto : EntityDto<int>
    {
        public int? ParentId { get; set; }
        public int ProductCategoryId { get; set; }
        public string Note { get; set; } = string.Empty; // Nội dung ghi chú
        public string ParentNote { get; set; } = string.Empty; // Nội dung ghi chú cha (nếu có)

        public string ProductCategoryName { get; set; } = string.Empty; // Tên danh mục sản phẩm

        public ProductCategoryDto ProductCategory { get; set; }   // Thông tin danh mục sản phẩm liên quan
    }
}