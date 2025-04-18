using Abp.Domain.Entities;

namespace InTN.Entities
{
    public class ProcessStepAction : Entity<int>
    {
        public int CurrentStepId { get; set; } // ID của bước hiện tại

        public int NextStepId { get; set; } // ID của bước tiếp theo

        public string Action { get; set; } // Hành động dẫn đến bước tiếp theo (ví dụ: "Phê duyệt", "Không phê duyệt")
    }
}
