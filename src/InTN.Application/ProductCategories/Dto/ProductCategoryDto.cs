using Abp.Application.Services.Dto;

namespace InTN.ProductCategories.Dto
{
    public class ProductCategoryDto : EntityDto<int>
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }
}