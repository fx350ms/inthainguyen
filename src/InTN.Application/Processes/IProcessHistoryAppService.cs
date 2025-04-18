using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.Processes.Dto;

namespace InTN.Processes
{
    public interface IProcessHistoryAppService : IAsyncCrudAppService<ProcessHistoryDto, int, PagedResultRequestDto, ProcessHistoryDto, ProcessHistoryDto>
    {
    }
}