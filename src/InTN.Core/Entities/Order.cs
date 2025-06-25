using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;


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
        public int PaymentStatus { get; set; }


        public int CustomerGender { get; set; }          // "Anh" / "Chị"
        public string CustomerType { get; set; }            // "Khách hàng", "Nội bộ", v.v.
        public string DeliveryMethod { get; set; }          // "Cty giao", "Khách lấy", "GH dịch vụ"
        public decimal? DeliveryFee { get; set; }           // Phí giao hàng
        public DateTime? ExpectedDeliveryDate { get; set; } // Ngày lấy hàng dự kiến

        public bool IsRequireTestSample { get; set; }       // Checkbox: test mẫu
        public bool IsExportInvoice { get; set; }           // Checkbox: xuất hoá đơn
        public bool IsStoreSample { get; set; }             // Checkbox: lưu mẫu
        public bool IsReceiveByOthers { get; set; }         // Checkbox: người khác nhận
        public string OtherRequirements { get; set; }       // Yêu cầu khác (textbox)
        public decimal? VatRate { get; set; }               // % VAT áp dụng
        public decimal? DiscountAmount { get; set; }        // Mã giảm giá hoặc tiền

        public string FileIds { get; set; } = string.Empty; // Danh sách ID các tệp đính kèm (dùng để lưu trữ ID của các tệp đính kèm liên quan đến đơn hàng)

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>(); // Danh sách chi tiết đơn hàng
    }

}
