using Abp.Domain.Entities.Auditing;
using System;


namespace InTN.Entities
{
    public class Order : FullAuditedEntity<int>
    {
        public string OrderCode { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Status { get; set; }
        public string Note { get; set; } = string.Empty;

        public string FileUrl { get; set; } = string.Empty; // Đường dẫn đến file đính kèm

        public string CustomerName { get; set; } = string.Empty; // Tên khách hàng
        public string CustomerAddress { get; set; } = string.Empty; // Địa chỉ khách hàng
        public string CustomerPhone { get; set; } = string.Empty; // Số điện thoại khách hàng   
        public string CustomerEmail { get; set; } = string.Empty; // Email khách hàng

        public decimal? TotalDeposit { get; set; } // Tổng số tiền đã đặt cọc
        public decimal? TotalAmount { get; set; } // Tổng số tiền của đơn hàng

    }
}
