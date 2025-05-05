using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;

using InTN.Customers.Dto;
using InTN.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using InTN.Commons;
using System;
using System.Linq;

namespace InTN.Customers
{
  
    public class CustomerAppService : AsyncCrudAppService<Customer, CustomerDto, int, PagedResultRequestDto, CustomerDto, CustomerDto>, ICustomerAppService
    {
        public CustomerAppService(IRepository<Customer> repository)
            : base(repository)
        {

        }


        public override Task<CustomerDto> GetAsync(EntityDto<int> input)
        {
            return base.GetAsync(input);
        }

        public async Task<List<CustomerDto>> GetAllListAsync()
        {
            var customers = await Repository.GetAllListAsync();
            return ObjectMapper.Map<List<CustomerDto>>(customers);
            //  return new ListResultDto<CustomerDto>(ObjectMapper.Map<List<CustomerDto>>(customers));
        }

        public async Task<List<OptionItemDto>> GetCustomerListForSelect(string q)
        {
            try
            {
                var query = await Repository.GetAllAsync() ;

                if (!string.IsNullOrEmpty(q))
                {
                    q = q.ToUpper();
                    query = query.Where(u => u.Name.ToUpper().Contains(q) || u.PhoneNumber.Contains(q));
                }

                return query.Select(u => new OptionItemDto
                {
                    id = u.Id.ToString(),
                    text = u.Name 
                }).ToList();


            }
            catch (Exception ex)
            {

            }
            return new List<OptionItemDto>();
        }

        public async Task UpdateCustomerCreditLimitAsync(int customerId, decimal? newCreditLimit)
        {
            var customer = await Repository.GetAsync(customerId);
            if (customer == null)
            {
                throw new Exception("Customer not found");
            }

            customer.CreditLimit = newCreditLimit;
            await Repository.UpdateAsync(customer);
        }


    }
}
