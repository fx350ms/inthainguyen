using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.ProductCategories.Dto;
using InTN.Entities;
using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace InTN.ProductCategories
{
    public class ProductCategoryAppService : AsyncCrudAppService<
        ProductCategory, // Entity chính
        ProductCategoryDto, // DTO chính
        int,                // Kiểu dữ liệu của khóa chính
        PagedResultRequestDto, // DTO cho phân trang
        ProductCategoryDto, // DTO cho tạo mới
        ProductCategoryDto>, // DTO cho cập nhật
        IProductCategoryAppService // Interface
    {
        public ProductCategoryAppService(IRepository<ProductCategory> repository)
           : base(repository)
        {
        }

        public async Task<List<ProductCategoryDto>> GetAllListAsync()
        {
            var data = (await Repository.GetAllListAsync()).OrderBy(u => u.Name);
            var result = ObjectMapper.Map<List<ProductCategoryDto>>(data);
            return result;
        }
    }
}