using Abp.Application.Services.Dto;

namespace InTN.Orders.Dto
{
    public class OrderDetailDto : EntityDto<int>
    {
        public int OrderId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal Width { get; set; }           // Chiều ngang (m)
        public decimal Height { get; set; }          // Chiều cao (m)
        public int Quantity { get; set; }
        public decimal Area { get; set; }            // Tổng diện tích
        public decimal UnitPrice { get; set; }       // Giá đơn vị (VD: theo m2)
        public decimal TotalPrice { get; set; }      // Tổng giá trị

        public int? FileId { get; set; }             // ID của tệp đính kèm liên quan đến chi tiết đơn hàng
        public string Note { get; set; } = string.Empty;
    }
}