using Abp.Application.Services;
using InTN.Processes.Dto;
using Abp.Application.Services.Dto;
using InTN.Entities;
using Abp.Domain.Repositories;

namespace InTN.Processes
{
    public class ProcessHistoryAppService : AsyncCrudAppService<ProcessHistory, ProcessHistoryDto, int, PagedResultRequestDto, ProcessHistoryDto, ProcessHistoryDto>, IProcessHistoryAppService
    {
        public ProcessHistoryAppService(IRepository<ProcessHistory> repository)
            : base(repository)
        {
        }
    }
}