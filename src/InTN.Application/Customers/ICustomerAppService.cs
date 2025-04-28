using Abp.Application.Services.Dto;
using Abp.Application.Services;
using InTN.Customers.Dto;

namespace InTN.Customers
{
    interface ICustomerAppService : IAsyncCrudAppService<CustomerDto, int, PagedResultRequestDto, CustomerDto, CustomerDto>
    {
    }
}
