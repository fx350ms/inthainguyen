using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.Brands.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.Brands
{
    public interface IBrandAppService : IAsyncCrudAppService<BrandDto, int, PagedResultRequestDto, BrandDto, BrandDto>
    {
        Task<List<BrandDto>> GetAllListAsync();
    }
}