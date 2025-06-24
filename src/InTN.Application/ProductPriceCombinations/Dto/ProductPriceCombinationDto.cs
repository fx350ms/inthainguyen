using Abp.Application.Services.Dto;

namespace InTN.ProductPriceCombinations.Dto
{
    public class ProductPriceCombinationDto : EntityDto<int>
    {
        public int ProductId { get; set; }
        public string PriceCombination { get; set; } // JSON string chứa các tổ hợp giá
    }
}