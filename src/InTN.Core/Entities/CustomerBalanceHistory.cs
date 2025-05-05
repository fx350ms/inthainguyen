using Abp.Domain.Entities.Auditing;

namespace InTN.Entities
{
    public class CustomerBalanceHistory : FullAuditedEntity<int>
    {
        public int CustomerId { get; set; } // Khóa ngoại liên kết với Customer
        public int TransactionId { get; set; } // ID giao dịch
        public int Type { get; set; } // Loại giao dịch (1: tăng công nợ, 2: giảm công nợ)
        public decimal Amount { get; set; } // Số tiền. Âm là công nợ, dương là số dư
        public decimal BalanceAfterTransaction { get; set; } // Số dư sau giao dịch
    }
}