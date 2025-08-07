using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.Printers.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.Printers
{
    public interface IPrinterAppService : IAsyncCrudAppService<
        PrinterDto, // DTO chính
        int,        // Kiểu dữ liệu của khóa chính
        PagedResultRequestDto, // DTO cho phân trang
        PrinterDto,      // DTO cho tạo mới
        PrinterDto>      // DTO cho cập nhật
    {
        Task<List<PrinterDto>> GetAllListAsync();
    }
}