using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
 

namespace InTN.Entities
{
   public class OrderLog : FullAuditedEntity<int>
    {
        public int OrderId { get; set; }
        public string Action { get; set; } // Hành động (Created, Updated, Completed, etc.)
        public string Note { get; set; } // Ghi chú thêm về hành động
        public string OldValue { get; set; } // Giá trị cũ (nếu có)
        public string NewValue { get; set; } // Giá trị mới (nếu có)
    }
}
