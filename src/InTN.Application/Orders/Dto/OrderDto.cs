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
        public int PaymentStatus { get; set; } // Trạng thái thanh toán

        public int DeliveryMethod { get; set; } // Phương thức giao hàng
        public DateTime? ExpectedDeliveryDate { get; set; } // Ngày giao hàng dự kiến
        public string OtherRequirements { get; set; } = string.Empty; // Yêu cầu khác
        public decimal? VatRate { get; set; } // % VAT áp dụng
        public decimal? VatAmount { get; set; } // Tiền thuế VAT
        public decimal? DiscountAmount { get; set; } // Số tiền giảm giá
        public decimal? TotalCustomerPay { get; set; } // Tổng số tiền khách phải trả
    }
}
