using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.ProductTypes.Dto;
using InTN.Entities;

namespace InTN.ProductTypes
{
    public class ProductTypeAppService : AsyncCrudAppService<ProductType, ProductTypeDto, int, PagedResultRequestDto, ProductTypeDto, ProductTypeDto>, IProductTypeAppService
    {
        public ProductTypeAppService(IRepository<ProductType> repository) : base(repository)
        {
        }
    }
}