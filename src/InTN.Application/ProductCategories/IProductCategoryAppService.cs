using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.ProductCategories.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.ProductCategories
{
    public interface IProductCategoryAppService : IAsyncCrudAppService<
        ProductCategoryDto, // DTO chính
        int,                // Kiểu dữ liệu của khóa chính
        PagedResultRequestDto, // DTO cho phân trang
        ProductCategoryDto, // DTO cho tạo mới
        ProductCategoryDto> // DTO cho cập nhật
    {
        // Các phương thức bổ sung nếu cần
        Task<List<ProductCategoryDto>> GetAllListAsync();
    }
}