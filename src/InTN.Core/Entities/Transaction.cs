using Abp.Domain.Entities.Auditing;
using System;

namespace InTN.Entities
{
    public class Transaction : FullAuditedEntity<int>
    {
        public string TransactionCode { get; set; } = string.Empty; // Mã giao dịch
        public int? CustomerId { get; set; } // Khóa ngoại liên kết với Customer
        public string CustomerName { get; set; } // Khóa ngoại liên kết với Customer
        public int? OrderId { get; set; } // Khóa ngoại liên kết với Order (nếu có)
        public decimal Amount { get; set; } // Số tiền giao dịch 
        public int TransactionType { get; set; } //1. Đặt cọc, 2. Thanh toán đơn hàng, 3. Thanh toán công nợ
        public string Description { get; set; } = string.Empty; // Mô tả giao dịch
        public byte[] FileContent { get; set; }
    }
}