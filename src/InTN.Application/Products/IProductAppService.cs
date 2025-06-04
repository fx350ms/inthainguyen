using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.Products.Dto;
using System.Threading.Tasks;

namespace InTN.Products
{
    public interface IProductAppService : IAsyncCrudAppService<
        ProductDto, // DTO chính
        int,        // Kiểu dữ liệu của khóa chính
        PagedResultRequestDto, // DTO cho phân trang
        CreateProductDto,      // DTO cho tạo mới
        ProductDto>            // DTO cho cập nhật
    {
        Task<ProductDto> GetProductDetailsAsync(int id); // Lấy chi tiết sản phẩm
    }
}