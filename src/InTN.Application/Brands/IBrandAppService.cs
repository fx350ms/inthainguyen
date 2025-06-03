using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.Brands.Dto;

namespace InTN.Brands
{
    public interface IBrandAppService : IAsyncCrudAppService<BrandDto, int, PagedResultRequestDto, BrandDto, BrandDto>
    {
    }
}