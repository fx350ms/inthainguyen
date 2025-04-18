using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.Processes.Dto;
 

namespace InTN.Processes
{
    public interface IProcessAppService : IAsyncCrudAppService<ProcessDto, int, PagedResultRequestDto, ProcessDto, ProcessDto>
    {
    }
}
