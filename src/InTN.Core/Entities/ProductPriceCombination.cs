using Abp.Domain.Entities;


namespace InTN.Entities
{

    /// <summary>
    /// Tổ hợp dữ liệu để tính giá
    /// </summary>
    public class ProductCombinationPrice : Entity<int>
    {
        public int ProductId { get; set; }
        public string PriceCombination { get; set; } // JSON string chứa các thuộc tính giá
    }
}
