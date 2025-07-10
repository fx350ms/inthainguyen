using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.Processes.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.Processes
{
    public interface IProcessStepAppService : IAsyncCrudAppService<ProcessStepDto, int, PagedResultRequestDto, ProcessStepDto, ProcessStepDto>
    {
        Task<List<ProcessStepDto>> GetStepsByProcessIdAsync(int processId);
    }
}