using Abp.Application.Services.Dto;
using InTN.Entities;
using InTN.ProductPriceCombinations.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace InTN.Orders.Dto
{
    public class CreateOrderItemDto : EntityDto<int>
    {
        public int ProductId { get; set; } // ID sản phẩm
        public string ProductName { get; set; } = string.Empty; // Tên sản phẩm
        public decimal UnitPrice { get; set; } // Giá trên một đơn vị sản phẩm
        public int Quantity { get; set; } // Số lượng sản phẩm
        public decimal TotalProductPrice { get; set; } // Tổng tiền sản phẩm (UnitPrice * Quantity)
        public string Note { get; set; } = string.Empty; // Ghi chú liên quan đến sản phẩm
        public int FileType { get; set; } // ID của tệp đính kèm liên quan đến sản phẩm
        public int? FileId { get; set; } // ID của tệp đính kèm liên quan đến sản phẩm
        public string FileUrl { get; set; } = string.Empty; // Đường dẫn đến tệp đính kèm liên quan đến chi tiết đơn hàng
        
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }     // Navigation



        public List<PropertyWithSelectedValueDto> Properties { get; set; } = new List<PropertyWithSelectedValueDto>(); // Danh sách thuộc tính sản phẩm với giá trị đã chọn
        public List<int> NoteIds { get; set; } = new List<int>(); // Danh sách ID ghi chú liên quan đến sản phẩm
    }
}