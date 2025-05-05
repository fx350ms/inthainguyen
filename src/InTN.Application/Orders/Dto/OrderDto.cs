using Abp.Application.Services.Dto;
using System;

namespace InTN.Orders.Dto
{
    public class OrderDto : EntityDto<int>
    {
        public string OrderCode { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Status { get; set; }
        public string Note { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        public string FileUrl { get; set; } = string.Empty; // Đường dẫn đến file đính kèm
        public decimal? TotalDeposit { get; set; } // Tổng số tiền đã đặt cọc
        public decimal? TotalAmount { get; set; } // Tổng số tiền của đơn hàng
    }
}
