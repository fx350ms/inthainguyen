using Abp.Application.Services.Dto;
using Abp.Application.Services;
using InTN.Customers.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.Customers
{
    public interface ICustomerAppService : IAsyncCrudAppService<CustomerDto, int, PagedResultRequestDto, CustomerDto, CustomerDto>
    {
        public Task<List<CustomerDto>> GetAllListAsync();
    }
}
