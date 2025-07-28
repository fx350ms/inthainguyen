using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.Commons;
using InTN.Entities;
using InTN.Processes.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InTN.Processes
{
    public class ProcessStepGroupAppService : AsyncCrudAppService<
        ProcessStepGroup, // Entity chính
        ProcessStepGroupDto, // DTO chính
        int,                 // Kiểu dữ liệu của khóa chính
        PagedResultRequestDto, // DTO cho phân trang
        ProcessStepGroupDto, // DTO cho tạo mới
        ProcessStepGroupDto>, // DTO cho cập nhật
        IProcessStepGroupAppService // Interface
    {
        public ProcessStepGroupAppService(IRepository<ProcessStepGroup, int> repository)
            : base(repository)
        {
        }


        public async Task<List<ProcessStepGroupDto>> GetAllListAsync()
        {
            var data = await Repository.GetAllListAsync();
            var result = ObjectMapper.Map<List<ProcessStepGroupDto>>(data);
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