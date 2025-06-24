using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.ProductProperties.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.ProductProperties
{
    public interface IProductPropertyAppService : IAsyncCrudAppService<ProductPropertyDto, int, PagedResultRequestDto, ProductPropertyDto, ProductPropertyDto>
    {
        public Task<List<ProductPropertyDto>> GetAllProductPropertiesAsync();
    }
}