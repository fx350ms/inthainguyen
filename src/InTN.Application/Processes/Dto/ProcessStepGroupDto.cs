using Abp.Application.Services.Dto;

namespace InTN.Processes.Dto
{
    public class ProcessStepGroupDto : EntityDto<int>
    {
        public string Name { get; set; } // Tên nhóm bước
        public string Description { get; set; } // Mô tả nhóm bước
    }

   
}