using Abp.Application.Services.Dto;

namespace InTN.Orders.Dto
{
    public class CreateOrderDetailDto : EntityDto<int>
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; } // ID sản phẩm
        public string ProductName { get; set; } = string.Empty; // Tên sản phẩm
        public decimal UnitPrice { get; set; } // Giá trên một đơn vị sản phẩm
        public int Quantity { get; set; } // Số lượng sản phẩm
        public decimal TotalProductPrice { get; set; } // Tổng tiền sản phẩm (UnitPrice * Quantity)
        public string Note { get; set; } = string.Empty; // Ghi chú liên quan đến sản phẩm
        public int? FileId { get; set; } // ID của tệp đính kèm liên quan đến sản phẩm
        public string Properties { get; set; } // Chuỗi JSON chứa danh sách thuộc tính sản phẩm với giá trị đã chọn
        public string NoteIds { get; set; } // // Danh sách ID ghi chú liên quan đến sản phẩm được ngăn cách nhau bởi dấu ,
    }
}