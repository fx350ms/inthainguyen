using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.ProductTypes.Dto;

namespace InTN.ProductTypes
{
    public interface IProductTypeAppService : IAsyncCrudAppService<ProductTypeDto, int, PagedResultRequestDto, ProductTypeDto, ProductTypeDto>
    {
    }
}