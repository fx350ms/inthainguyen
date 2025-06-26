using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.Commons;
using InTN.Entities;
using InTN.ProductNotes.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.ProductNotes
{
    public interface IProductNoteAppService : IAsyncCrudAppService<
        ProductNoteDto, // DTO chính
        int,            // Kiểu dữ liệu của khóa chính
        PagedResultRequestDto, // DTO cho phân trang
        ProductNoteDto, // DTO cho tạo mới
        ProductNoteDto> // DTO cho cập nhật
    {
        Task<List<OptionItemDto>> FilterAndSearchProductNoteAsync(FilterandSearchProductNoteRequestDto input);
        Task<List<ProductNote>> GetAllListAsync();
        Task<PagedResultDto<ProductNoteDto>> GetDataAsync(PagedProductNoteResultRequestDto input);
        Task<List<ProductNoteDto>> GetNotesByProductIdAsync(int productId);

    }
}