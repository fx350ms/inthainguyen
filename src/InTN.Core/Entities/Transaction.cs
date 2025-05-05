using Abp.Domain.Entities.Auditing;
using System;

namespace InTN.Entities
{
    public class Transaction : FullAuditedEntity<int>
    {
        public int? CustomerId { get; set; } // Khóa ngoại liên kết với Customer

        public int? OrderId { get; set; } // Khóa ngoại liên kết với Order (nếu có)

        public decimal Amount { get; set; } // Số tiền giao dịch (âm nếu là trả nợ, dương nếu là tăng công nợ)
        public string Description { get; set; } = string.Empty; // Mô tả giao dịch
    }
}