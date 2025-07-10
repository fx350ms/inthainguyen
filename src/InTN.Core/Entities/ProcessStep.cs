using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace InTN.Entities
{
    public class ProcessStep : Entity<int>
    {
        public int ProcessId { get; set; } // Khóa ngoại liên kết với Process
        public string Name { get; set; } // Tên bước
        public string RoleIds { get; set; } // Danh sách các role được làm bước này
        public int OrderStatus { get; set; } // Trạng thái của bước (ví dụ: 0 - Chưa bắt đầu, 1 - Đang thực hiện, 2 - Hoàn thành, 3 - Bị hủy)

        public string NextStepIds { get; set; } // Danh sách các ID của bước trước nếu có

        [ForeignKey("ProcessId")]
        public virtual Process Process { get; set; } // Liên kết với Process
    }
}
