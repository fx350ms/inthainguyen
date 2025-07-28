using InTN.Processes.Dto;
using System.Collections.Generic;

namespace InTN.Web.Models.Processes
{
    public class ProcessConfigViewModel
    {
        public ProcessDto Process { get; set; }
        public List<ProcessStepGroupDto> ProcessStepGroups { get; set; }
        public List<ProcessStepDto> ProcessSteps { get; set; }
    }
}
