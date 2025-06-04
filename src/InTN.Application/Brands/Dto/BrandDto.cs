using Abp.Application.Services.Dto;

namespace InTN.Brands.Dto
{
    public class BrandDto : EntityDto<int>
    {
        public string Name { get; set; } // Tên thương hiệu
        public string Description { get; set; } // Mô tả thương hiệu
    }
}