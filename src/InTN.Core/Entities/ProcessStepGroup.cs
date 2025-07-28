using Abp.Domain.Entities;

namespace InTN.Entities
{
    public class ProcessStepGroup : Entity<int>
    {
        public string Name { get; set; } // Tên nhóm bước
        public string Description { get; set; } // Mô tả nhóm bước
    }
}
