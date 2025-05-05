using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.CustomerBalanceHistories.Dto;
using InTN.Customers.Dto;
using InTN.Customers;
using InTN.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.CustomerBalanceHistories
{
    public class CustomerBalanceHistoryAppService : AsyncCrudAppService<CustomerBalanceHistory, CustomerBalanceHistoryDto, int, PagedResultRequestDto, CustomerBalanceHistoryDto, CustomerBalanceHistoryDto>,   ICustomerBalanceHistoryAppService
    {
        public CustomerBalanceHistoryAppService(IRepository<CustomerBalanceHistory> repository)
          : base(repository)
        {

        }
 
    }
}