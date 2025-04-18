using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace InTN.Entities
{
    public class Process : FullAuditedEntity<int>
    {
        public string Name { get; set; } // Tên quy trình
        public string Description { get; set; } // Mô tả quy trình
        public string Design { get; set; } // Thiết kế quy trình (có thể là JSON hoặc XML mô tả quy trình)
        public int Status { get; set; } // Trạng thái 
     
    }
}
