using Abp.Application.Services;
using InTN.Roles.Dto;
using InTN.Processes.Dto;
using Abp.Application.Services.Dto;
using InTN.Entities;
using Abp.Domain.Repositories;
using InTN.Authorization.Roles;
using System.Threading.Tasks;
using InTN.ProductTypes.Dto;
using System.Collections.Generic;

namespace InTN.Processes
{
    public class ProcessAppService : AsyncCrudAppService<Process, ProcessDto, int, PagedResultRequestDto, ProcessDto, ProcessDto>, IProcessAppService
    {
        public ProcessAppService(IRepository<Process> repository )
            : base(repository)
        {
           
        }

        public async Task<List<ProcessDto>> GetAllListAsync()
        {
            var data = await Repository.GetAllListAsync();
            var result = ObjectMapper.Map<List<ProcessDto>>(data);
            return result;
        }
    }
}
