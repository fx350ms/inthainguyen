using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

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



        // Các trường bổ sung từ Entity
        public int CustomerGender { get; set; } // "Anh" / "Chị"
        public string CustomerType { get; set; } = string.Empty; // "Khách hàng", "Nội bộ", v.v.

        public bool IsRequireDesign { get; set; } // Checkbox: Yêu cầu design
        public bool IsRequireTestSample { get; set; } // Checkbox: test mẫu
        public bool IsExportInvoice { get; set; } // Checkbox: xuất hoá đơn
        public bool IsStoreSample { get; set; } // Checkbox: lưu mẫu
        public bool IsReceiveByOthers { get; set; } // Checkbox: người khác nhận

        public string FileIds { get; set; } = string.Empty; // Danh sách ID file đính kèm

        public List<OrderDetailDto> OrderDetails { get; set; } = new(); // Danh sách chi tiết đơn hàng

        public decimal? TotalProductAmount { get; set; } // Tổng số tiền của các sản phẩm
        public decimal? DeliveryFee { get; set; } // Phí giao hàng


        public int? ProcessId { get; set; } // ID của quy trình liên quan đến đơn hàng
    }
}
