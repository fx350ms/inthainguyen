using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;

using InTN.Customers.Dto;
using InTN.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InTN.Customers
{
  
    public class CustomerAppService : AsyncCrudAppService<Customer, CustomerDto, int, PagedResultRequestDto, CustomerDto, CustomerDto>, ICustomerAppService
    {
        public CustomerAppService(IRepository<Customer> repository)
            : base(repository)
        {

        }


        public async Task<List<CustomerDto>> GetAllListAsync()
        {
            var customers = await Repository.GetAllListAsync();
            return ObjectMapper.Map<List<CustomerDto>>(customers);
            //  return new ListResultDto<CustomerDto>(ObjectMapper.Map<List<CustomerDto>>(customers));
        }
    }
}
