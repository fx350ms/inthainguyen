using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.Commons;
using InTN.Processes.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.Processes
{
    public interface IProcessStepGroupAppService : IAsyncCrudAppService<
        ProcessStepGroupDto, // DTO chính
        int,                 // Kiểu dữ liệu của khóa chính
        PagedResultRequestDto, // DTO cho phân trang
        ProcessStepGroupDto, // DTO cho tạo mới
        ProcessStepGroupDto> // DTO cho cập nhật
    {
        Task<List<ProcessStepGroupDto>> GetAllListAsync();
        Task<List<OptionItemDto>> GetAllListForSelectAsync();
    }
}