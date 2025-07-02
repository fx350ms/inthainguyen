using System.Collections.Generic;

namespace InTN.ProductPriceCombinations.Dto
{
    public class SavePriceCombinationsDto
    {
        public int ProductId { get; set; }
        public List<PriceCombinationDto> PriceCombinations { get; set; }
        public List<PropertyWithValuesDto> Properties { get; set; } // Danh sách các thuộc tính của sản phẩm 
    }

    public class PriceCombinationDto
    {
        public List<CombinationItemDto> Combinations { get; set; }
        public decimal Price { get; set; }
    }

    public class CombinationItemDto
    {
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public string Value { get; set; }
    }

    public class PropertyWithValuesDto
    {
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public List<string> Values { get; set; } // Danh sách các giá trị của thuộc tính
    }
}