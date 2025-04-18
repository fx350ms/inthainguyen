using Abp.Application.Services.Dto;

namespace InTN.Processes.Dto
{
    public class ProcessStepActionDto : EntityDto<int>
    {
        public int CurrentStepId { get; set; } // ID của bước hiện tại
        public int NextStepId { get; set; } // ID của bước tiếp theo
        public string Action { get; set; } // Hành động dẫn đến bước tiếp theo
    }
}