using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.ProductTypes.Dto;
using InTN.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InTN.ProductTypes
{
    public class ProductTypeAppService : AsyncCrudAppService<ProductType, ProductTypeDto, int, PagedResultRequestDto, ProductTypeDto, ProductTypeDto>, IProductTypeAppService
    {
        public ProductTypeAppService(IRepository<ProductType> repository) : base(repository)
        {
        }

        public async Task<List<ProductTypeDto>> GetAllListAsync()
        {
            var data = await Repository.GetAllListAsync();
            var result = ObjectMapper.Map<List<ProductTypeDto>>(data);
            return result;
        }
    }
}