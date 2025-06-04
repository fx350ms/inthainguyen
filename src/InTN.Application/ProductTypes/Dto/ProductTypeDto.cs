using Abp.Application.Services.Dto;

namespace InTN.ProductTypes.Dto
{
    public class ProductTypeDto : EntityDto<int>
    {
        public string Name { get; set; } // Tên loại sản phẩm
        public string Description { get; set; } // Mô tả loại sản phẩm
    }
}