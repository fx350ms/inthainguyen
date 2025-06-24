using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.ProductProperties.Dto;
using InTN.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.ProductProperties
{
    public class ProductPropertyAppService : AsyncCrudAppService<ProductProperty, ProductPropertyDto, int, PagedResultRequestDto, ProductPropertyDto, ProductPropertyDto>, IProductPropertyAppService
    {
        public ProductPropertyAppService(IRepository<ProductProperty> repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets all product properties from the repository and maps them to a list of ProductPropertyDto.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductPropertyDto>> GetAllProductPropertiesAsync()
        {
            var productProperties = await Repository.GetAllAsync();
            return ObjectMapper.Map<List<ProductPropertyDto>>(productProperties);
        }
    }
}