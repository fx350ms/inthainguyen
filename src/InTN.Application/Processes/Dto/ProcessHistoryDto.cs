using Abp.Application.Services.Dto;
using System;

namespace InTN.Processes.Dto
{
    public class ProcessHistoryDto : EntityDto<int>
    {
        public int ProcessId { get; set; } // Khóa ngoại liên kết với Process
        public string Action { get; set; } // Hành động (Created, Updated, Completed, etc.)
        public string PerformedBy { get; set; } // Người thực hiện hành động
        public DateTime ActionDate { get; set; } // Ngày thực hiện hành động
    }
}