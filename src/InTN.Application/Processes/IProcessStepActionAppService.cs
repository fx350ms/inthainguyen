using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.Processes.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.Processes
{
    public interface IProcessStepActionAppService : IAsyncCrudAppService<ProcessStepActionDto, int, PagedResultRequestDto, ProcessStepActionDto, ProcessStepActionDto>
    {
       
    }
}