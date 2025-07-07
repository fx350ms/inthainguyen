using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTN.Entities
{
    public class OrderDetail : FullAuditedEntity<int>
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; } // ID sản phẩm, dịch vụ
        public string ProductName { get; set; } = string.Empty; // Tên sản phẩm, dịch vụ
        public decimal UnitPrice { get; set; } // Giá trên một đơn vị sản phẩm
        public int Quantity { get; set; }
        public decimal TotalProductPrice { get; set; } // Tổng tiền sản phẩm (UnitPrice * Quantity)
        public int? FileId { get; set; } // ID của tệp đính kèm liên quan đến chi tiết đơn hàng
        public string FileUrl { get; set; } = string.Empty; // Đường dẫn đến tệp đính kèm liên quan đến chi tiết đơn hàng

        public int FileType { get; set; } = 2;// Loại tệp đính kèm(1. Upload, 2. Link) 

        public string Note { get; set; } = string.Empty;
        public string Properties { get; set; } // Chuỗi JSON chứa danh sách thuộc tính sản phẩm với giá trị đã chọn
        public string NoteIds { get; set; } // // Danh sách ID ghi chú liên quan đến sản phẩm được ngăn cách nhau bởi dấu 

        [ForeignKey("OrderId")]
        [NotMapped]
        public virtual Order Order { get; set; }     // Navigation
    }
}
