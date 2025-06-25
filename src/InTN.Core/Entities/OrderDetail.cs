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
        public string ServiceName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal Width { get; set; }           // Chiều ngang (m)
        public decimal Height { get; set; }          // Chiều cao (m)
        public int Quantity { get; set; }
        public decimal Area => Width * Height * Quantity; // Tổng diện tích
        public decimal UnitPrice { get; set; }       // Giá đơn vị (VD: theo m2)
        public decimal TotalPrice => Area * UnitPrice;

        public int? FileId { get; set; } // ID của tệp đính kèm liên quan đến chi tiết đơn hàng
        public string Note { get; set; } = string.Empty;

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }     // Navigation
    }
}
