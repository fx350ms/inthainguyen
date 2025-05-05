using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTN.Transactions.Dto
{
    public class CreateTransactionDto : FullAuditedEntityDto<int>
    {
        public string TransactionCode { get; set; } // Mã giao dịch
        public int? CustomerId { get; set; } // ID của khách hàng
        public string CustomerName { get; set; } // Tên khách hàng
        public int? OrderId { get; set; } // ID của đơn hàng (nếu có)
        public decimal Amount { get; set; } // Số tiền giao dịch
        public string Description { get; set; } // Mô tả giao dịch
        public int TransactionType { get; set; } //1. Đặt cọc, 2. Thanh toán đơn hàng, 3. Thanh toán công nợ
        public byte[] FileContent { get; set; }

        public List<IFormFile> Attachments { get; set; }

    }
}
