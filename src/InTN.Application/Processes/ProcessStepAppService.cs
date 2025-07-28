using Abp.Application.Services;
using InTN.Processes.Dto;
using Abp.Application.Services.Dto;
using InTN.Entities;
using Abp.Domain.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using InTN.Brands.Dto;
using InTN.Commons;


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
            //var steps = await Repository.GetAllListAsync(step => step.ProcessId == processId);
            //return ObjectMapper.Map<List<ProcessStepDto>>(steps);

            return null;
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


        public async Task<List<ProcessStepDto>> GetAllListAsync()
        {
            var data = await Repository.GetAllListAsync();
            var result = ObjectMapper.Map<List<ProcessStepDto>>(data);
            return result;
        }

        public async Task<List<OptionItemDto>> GetAllListForSelectAsync()
        {
            var data = (await Repository.GetAllListAsync())
                .Select(x => new OptionItemDto
                {
                    id = x.Id.ToString(),
                    text = x.Name
                })
                .ToList();
            return data;
        }
    }
}