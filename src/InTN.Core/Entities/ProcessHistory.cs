using Abp.Domain.Entities;
using System;

namespace InTN.Entities
{
   public class ProcessHistory : Entity<int>
    {
        public int ProcessId { get; set; } // Khóa ngoại liên kết với Process
        public string Action { get; set; } // Hành động (Created, Updated, Completed, etc.)
        public string PerformedBy { get; set; } // Người thực hiện hành động
        public DateTime ActionDate { get; set; } // Ngày thực hiện hành động
     
    }
}
