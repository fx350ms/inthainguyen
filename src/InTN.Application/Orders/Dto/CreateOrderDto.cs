using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace InTN.Orders.Dto
{
    public class CreateOrderDto : EntityDto<int>
    {
        public bool IsCasualCustomer { get; set; } = false; // true: Khách hàng vãng lai, false: Khách hàng đã có trong hệ thống
        public string OrderCode { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Status { get; set; }
        public string Note { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty; // Đường dẫn đến file đính kèm

        public string CustomerName { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        // Các trường bổ sung từ Order.cs
        public decimal? TotalDeposit { get; set; } // Tổng số tiền đã đặt cọc
        public decimal? TotalAmount { get; set; } // Tổng số tiền của đơn hàng
        public int PaymentStatus { get; set; }

        public int CustomerGender { get; set; } // "Anh" / "Chị"
        public string CustomerType { get; set; } = string.Empty; // "Khách hàng", "Nội bộ", v.v.
        public string DeliveryMethod { get; set; } = string.Empty; // "Cty giao", "Khách lấy", "GH dịch vụ"
        public decimal? DeliveryFee { get; set; } // Phí giao hàng
        public DateTime? ExpectedDeliveryDate { get; set; } // Ngày lấy hàng dự kiến

        public bool IsRequireTestSample { get; set; } // Checkbox: test mẫu
        public bool IsExportInvoice { get; set; } // Checkbox: xuất hoá đơn
        public bool IsStoreSample { get; set; } // Checkbox: lưu mẫu
        public bool IsReceiveByOthers { get; set; } // Checkbox: người khác nhận
        public string OtherRequirements { get; set; } = string.Empty; // Yêu cầu khác (textbox)
        public decimal? VatRate { get; set; } // % VAT áp dụng
        public decimal? DiscountAmount { get; set; } // Mã giảm giá hoặc tiền

        public string FileIds { get; set; } = string.Empty; // Danh sách ID các tệp đính kèm (dùng để lưu trữ ID của các tệp đính kèm liên quan đến đơn hàng)

        public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>(); // Danh sách chi tiết đơn hàng
    }

   
}
