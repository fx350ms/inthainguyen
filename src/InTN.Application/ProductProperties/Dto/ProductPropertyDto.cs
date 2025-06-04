using Abp.Application.Services.Dto;

namespace InTN.ProductProperties.Dto
{
    public class ProductPropertyDto : EntityDto<int>
    {
        public string Name { get; set; } // Tên thuộc tính sản phẩm
    }
}