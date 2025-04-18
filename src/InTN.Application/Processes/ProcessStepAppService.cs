using Abp.Application.Services;
using InTN.Processes.Dto;
using Abp.Application.Services.Dto;
using InTN.Entities;
using Abp.Domain.Repositories;

namespace InTN.Processes
{
    public class ProcessStepAppService : AsyncCrudAppService<ProcessStep, ProcessStepDto, int, PagedResultRequestDto, ProcessStepDto, ProcessStepDto>, IProcessStepAppService
    {
        public ProcessStepAppService(IRepository<ProcessStep> repository)
            : base(repository)
        {
        }
    }
}