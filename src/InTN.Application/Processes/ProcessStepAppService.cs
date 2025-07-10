using Abp.Application.Services;
using InTN.Processes.Dto;
using Abp.Application.Services.Dto;
using InTN.Entities;
using Abp.Domain.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;

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
    }
}