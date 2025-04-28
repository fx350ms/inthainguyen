using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;

using InTN.Customers.Dto;
using InTN.Entities;

namespace InTN.Customers
{
  
    public class CustomerAppService : AsyncCrudAppService<Customer, CustomerDto, int, PagedResultRequestDto, CustomerDto, CustomerDto>, ICustomerAppService
    {
        public CustomerAppService(IRepository<Customer> repository)
            : base(repository)
        {

        }
    }
}
