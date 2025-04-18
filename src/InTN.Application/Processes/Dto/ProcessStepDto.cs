using Abp.Application.Services.Dto;

namespace InTN.Processes.Dto
{
    public class ProcessStepDto : EntityDto<int>
    {
        public int ProcessId { get; set; } // Khóa ngoại liên kết với Process
        public string Name { get; set; } // Tên bước
        public int RoleId { get; set; } // ID của vai trò thực hiện bước này
        public int? PreviousStepId { get; set; } // ID của bước trước đó (nếu có)
    }
}