using Abp.Application.Services;
using InTN.Processes.Dto;
using Abp.Application.Services.Dto;
using InTN.Entities;
using Abp.Domain.Repositories;

namespace InTN.Processes
{
    public class ProcessStepActionAppService : AsyncCrudAppService<ProcessStepAction, ProcessStepActionDto, int, PagedResultRequestDto, ProcessStepActionDto, ProcessStepActionDto>, IProcessStepActionAppService
    {
        public ProcessStepActionAppService(IRepository<ProcessStepAction> repository)
            : base(repository)
        {
        }
    }
}