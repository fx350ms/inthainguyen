using System.Collections.Generic;

namespace InTN.ProductPriceCombinations.Dto
{
    public class PropertyWithValuesDto
    {
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public List<string> Values { get; set; } // Danh sách các giá trị của thuộc tính
    }


    public class PropertyWithSelectedValueDto
    {
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public string Value { get; set; } // Giá trị của thuộc tính
    }
}