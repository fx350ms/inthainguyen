using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.ProductProperties.Dto;
using InTN.Entities;

namespace InTN.ProductProperties
{
    public class ProductPropertyAppService : AsyncCrudAppService<ProductProperty, ProductPropertyDto, int, PagedResultRequestDto, ProductPropertyDto, ProductPropertyDto>, IProductPropertyAppService
    {
        public ProductPropertyAppService(IRepository<ProductProperty> repository) : base(repository)
        {
        }
    }
}