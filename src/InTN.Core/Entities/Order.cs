using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;


namespace InTN.Entities
{
    public class Order : FullAuditedEntity<int>
    {
        public string OrderCode { get; set; } = string.Empty;
        public int? CustomerId { get; set; }

        public DateTime OrderDate { get; set; } // Ngày tạo đơn hàng
        /// <summary>
        /// Trạng thái đơn hàng
        /// </summary>
        public int Status { get; set; }
        public string Note { get; set; } = string.Empty;

        public string FileUrl { get; set; } = string.Empty; // Đường dẫn đến file đính kèm

        public string CustomerName { get; set; } = string.Empty; // Tên khách hàng
        public string CustomerAddress { get; set; } = string.Empty; // Địa chỉ khách hàng
        public string CustomerPhone { get; set; } = string.Empty; // Số điện thoại khách hàng   
        public string CustomerEmail { get; set; } = string.Empty; // Email khách hàng
        public int PaymentStatus { get; set; }

        public int CustomerGender { get; set; }          // "Anh" / "Chị"
        public string CustomerType { get; set; }            // "Khách hàng", "Nội bộ", v.v.
        public int DeliveryMethod { get; set; }          // "Cty giao", "Khách lấy", "GH dịch vụ"
      
        public DateTime? ExpectedDeliveryDate { get; set; } // Ngày lấy hàng dự kiến

        public bool IsRequireDesign { get; set; } // Checkbox: Yêu cầu design
        public bool IsRequireTestSample { get; set; }       // Checkbox: test mẫu
        public bool IsExportInvoice { get; set; }           // Checkbox: xuất hoá đơn
        public bool IsStoreSample { get; set; }             // Checkbox: lưu mẫu
        public bool IsReceiveByOthers { get; set; }         // Checkbox: người khác nhận
        public string OtherRequirements { get; set; }       // Yêu cầu khác (textbox)
       

        public string FileIds { get; set; } = string.Empty; // Danh sách ID các tệp đính kèm (dùng để lưu trữ ID của các tệp đính kèm liên quan đến đơn hàng)

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>(); // Danh sách chi tiết đơn hàng


        public decimal? TotalProductAmount { get; set; } // Tổng số tiền của các sản phẩm trong đơn hàng
        public decimal? TotalDeposit { get; set; } // Tổng số tiền đã đặt cọc
        public decimal? DeliveryFee { get; set; }           // Phí giao hàng
        public decimal? VatRate { get; set; }               // % VAT áp dụng
        public decimal? VatAmount { get; set; } // Tiền thuế VAT
        public decimal? DiscountAmount { get; set; }        // Số tiền giảm giá 
        public decimal? TotalAmount { get; set; } // Tổng số tiền của đơn hàng
        public decimal? TotalCustomerPay { get; set; } // Tổng số tiền khách phải trả
    }

}
