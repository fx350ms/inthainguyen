using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.ProductProperties.Dto;

namespace InTN.ProductProperties
{
    public interface IProductPropertyAppService : IAsyncCrudAppService<ProductPropertyDto, int, PagedResultRequestDto, ProductPropertyDto, ProductPropertyDto>
    {
    }
}