using Abp.Domain.Entities;
using System;
using System.Collections.Generic;

namespace InTN.Entities
{
    public class ProcessStep : Entity<int>
    {
        public int ProcessId { get; set; } // Khóa ngoại liên kết với Process
        public string Name { get; set; } // Tên bước
        public int RoleId { get; set; } // ID của vai trò thực hiện bước này
        public Process Process { get; set; } // Điều hướng ngược về Process
        public int? PreviousStepId { get; set; } // ID của bước trước đó (nếu có)
        public ProcessStep PreviousStep { get; set; } // Điều hướng đến bước trước đó

       
    }
}
