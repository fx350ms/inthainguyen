using InTN.ProductPriceCombinations.Dto;
using InTN.ProductProperties.Dto;
using System.Collections.Generic;

namespace InTN.Web.Models.Products
{
    public class ProductEditPriceCombinationModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } // Tên sản phẩm
        public List<ProductPropertyDto> ProductProperties { get; set; } // Danh sách các thuộc tính sản phẩm
        public string PriceCombination { get; set; } // JSON string chứa các thuộc tính giá
        public List<PriceCombinationDto> PriceCombinations { get; set; } // Danh sách các kết hợp giá

    }
}
