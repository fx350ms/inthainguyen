using Abp.Application.Services;
using InTN.Processes.Dto;
using Abp.Application.Services.Dto;
using InTN.Entities;
using Abp.Domain.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;


namespace InTN.Processes
{
    public class ProcessStepAppService : AsyncCrudAppService<ProcessStep, ProcessStepDto, int, PagedResultRequestDto, ProcessStepDto, ProcessStepDto>, IProcessStepAppService
    {
        public ProcessStepAppService(IRepository<ProcessStep> repository)
            : base(repository)
        {
        }

        public async Task<List<ProcessStepDto>> GetStepsByProcessIdAsync(int processId)
        {
            var steps = await Repository.GetAllListAsync(step => step.ProcessId == processId);
            return ObjectMapper.Map<List<ProcessStepDto>>(steps);
        }

        public async Task<List<ProcessStepDto>> GetNextStepsAsync(int currentStepId)
        {
            var processStep = await Repository.FirstOrDefaultAsync(x => x.Id == currentStepId);

            if (processStep != null && !string.IsNullOrEmpty(processStep.NextStepIds))
            {
                var nextStepIds = processStep.NextStepIds.Split(",");
                var nextSteps = await Repository.GetAllListAsync(x => nextStepIds.Contains(x.Id.ToString()));
                return ObjectMapper.Map<List<ProcessStepDto>>(nextSteps);
            }

            return new List<ProcessStepDto>();

        }
    }
}