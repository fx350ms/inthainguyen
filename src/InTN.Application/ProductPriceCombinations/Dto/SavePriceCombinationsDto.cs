using System.Collections.Generic;

namespace InTN.ProductPriceCombinations.Dto
{
    public class SavePriceCombinationsDto
    {
        public int ProductId { get; set; }
        public List<PriceCombinationDto> PriceCombinations { get; set; }
    }

    public class PriceCombinationDto
    {
        public List<CombinationItemDto> Combination { get; set; }
        public decimal Price { get; set; }
    }

    public class CombinationItemDto
    {
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public string Value { get; set; }
    }
}