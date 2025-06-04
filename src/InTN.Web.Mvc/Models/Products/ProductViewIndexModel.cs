using InTN.Brands.Dto;
using InTN.ProductCategories.Dto;
using InTN.Products.Dto;
using InTN.ProductTypes.Dto;
using InTN.Suppliers.Dto;
using System.Collections.Generic;

namespace InTN.Web.Models.Products
{
    public class ProductIndexModel
    {
        public List<ProductTypeDto> ProductTypes { get; set; }
        public List<ProductCategoryDto> ProductCategories { get; set; }
        public List<SupplierDto> Suppliers { get; set; }
        public List<BrandDto> Brands { get; set; }
    }

    public class ProductCreateModel
    {
        public List<ProductTypeDto> ProductTypes { get; set; }
        public List<ProductCategoryDto> ProductCategories { get; set; }
        public List<SupplierDto> Suppliers { get; set; }
        public List<BrandDto> Brands { get; set; }
        public CreateProductDto Dto { get; set; }
    }
}
