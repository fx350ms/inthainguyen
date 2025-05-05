using Abp.Application.Services.Dto;
using System;

namespace InTN.Transactions.Dto
{
    public class TransactionDto : EntityDto<int>
    {
        public int? CustomerId { get; set; } // ID của khách hàng
        public string CustomerName { get; set; } // Tên khách hàng
        public int? OrderId { get; set; } // ID của đơn hàng (nếu có)
        public decimal Amount { get; set; } // Số tiền giao dịch
        public string Description { get; set; } // Mô tả giao dịch
        public DateTime TransactionDate { get; set; } // Ngày giao dịch
    }
}