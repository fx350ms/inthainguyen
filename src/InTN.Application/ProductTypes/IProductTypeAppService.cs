using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.ProductTypes.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.ProductTypes
{
    public interface IProductTypeAppService : IAsyncCrudAppService<ProductTypeDto, int, PagedResultRequestDto, ProductTypeDto, ProductTypeDto>
    {
        Task<List<ProductTypeDto>> GetAllListAsync();
    }
}