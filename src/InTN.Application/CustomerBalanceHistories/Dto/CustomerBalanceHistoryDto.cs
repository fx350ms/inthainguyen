using Abp.Application.Services.Dto;
using System;

namespace InTN.CustomerBalanceHistories.Dto
{
    public class CustomerBalanceHistoryDto : FullAuditedEntityDto<int>
    {
        public int CustomerId { get; set; } // ID khách hàng
        public int TransactionId { get; set; } // ID giao dịch
        public int Type { get; set; } // Loại giao dịch (1: tăng công nợ, 2: giảm công nợ)
        public decimal Amount { get; set; } // Số tiền
        public decimal BalanceAfterTransaction { get; set; } // Số dư sau giao dịch
    }
}