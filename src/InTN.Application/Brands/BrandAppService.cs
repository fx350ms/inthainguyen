using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.Brands.Dto;
using InTN.Entities;

namespace InTN.Brands
{
    public class BrandAppService : AsyncCrudAppService<Brand, BrandDto, int, PagedResultRequestDto, BrandDto, BrandDto>, IBrandAppService
    {
        public BrandAppService(IRepository<Brand> repository) : base(repository)
        {
        }

        
    }
}