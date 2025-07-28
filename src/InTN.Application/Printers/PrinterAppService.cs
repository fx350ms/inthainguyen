using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.Entities;
using InTN.Printers.Dto;

namespace InTN.Printers
{
    public class PrinterAppService : AsyncCrudAppService<
        Printer, // Entity chính
        PrinterDto, // DTO chính
        int,        // Kiểu dữ liệu của khóa chính
        PagedResultRequestDto, // DTO cho phân trang
        PrinterDto,      // DTO cho tạo mới
        PrinterDto>,     // DTO cho cập nhật
        IPrinterAppService      // Interface
    {
        public PrinterAppService(IRepository<Printer, int> repository)
            : base(repository)
        {
        }
    }
}