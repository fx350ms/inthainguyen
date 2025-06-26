using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.Brands.Dto;
using InTN.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.Brands
{
    public class BrandAppService : AsyncCrudAppService<Brand, BrandDto, int, PagedResultRequestDto, BrandDto, BrandDto>, IBrandAppService
    {
        public BrandAppService(IRepository<Brand> repository) : base(repository)
        {
        }

        public async Task<List<BrandDto>> GetAllListAsync()
        {
            var data = await Repository.GetAllListAsync();
            var result = ObjectMapper.Map<List<BrandDto>>(data);
            return result;
        }
    }
}