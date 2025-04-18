using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.Processes.Dto;

namespace InTN.Processes
{
    public interface IProcessStepAppService : IAsyncCrudAppService<ProcessStepDto, int, PagedResultRequestDto, ProcessStepDto, ProcessStepDto>
    {
    }
}