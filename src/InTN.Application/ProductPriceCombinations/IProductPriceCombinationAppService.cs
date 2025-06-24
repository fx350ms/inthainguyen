using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.ProductPriceCombinations.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.ProductPriceCombinations
{
    public interface IProductPriceCombinationAppService : IAsyncCrudAppService<
        ProductPriceCombinationDto, // DTO chính
        int,                        // Kiểu dữ liệu của khóa chính
        PagedResultRequestDto,      // DTO cho phân trang
        ProductPriceCombinationDto, // DTO cho tạo mới
        ProductPriceCombinationDto> // DTO cho cập nhật
    {
        Task<List<ProductPriceCombinationDto>> GetAllProductPriceCombinationsAsync();
    }
}