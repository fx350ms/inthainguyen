using InTN.ProductCategories.Dto;
using System.Collections.Generic;

namespace InTN.Web.Models.ProductCategories
{
    public class CreateProductCategoryModel
    {
        public List<ProductCategoryDto> ParentCategories { get; set; } = new List<ProductCategoryDto>();
        public ProductCategoryDto ProductCategory { get; set; } = new ProductCategoryDto();
    }
}
