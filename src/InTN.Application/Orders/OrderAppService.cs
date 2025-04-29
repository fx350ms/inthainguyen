using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;

using InTN.Orders.Dto;
using InTN.Entities;
using System.Threading.Tasks;
using System;

namespace InTN.Orders
{
    public class OrderAppService : AsyncCrudAppService<Order, OrderDto, int, PagedResultRequestDto, OrderDto, OrderDto>, IOrderAppService
    {
        public OrderAppService(IRepository<Order> repository)
            : base(repository)
        {
        }

        public async Task<OrderDto> CreateOrderWithAttachmentAsync(OrderDto input)
        {
            return await base.CreateAsync(input);
        }


        public async Task<Order> CreateNewAsync(CreateOrderDto input)
        {
            // Validate input
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), "Input cannot be null");
            }
            if (string.IsNullOrEmpty(input.OrderCode))
            {
                throw new ArgumentException("Order code cannot be null or empty", nameof(input.OrderCode));
            }

            input.OrderDate = DateTime.Now; // Set the current date and time as the order date
            input.Status = (int)OrderStatus.ReceivedRequest; // Set default status to 0 (Pending)

            var order = ObjectMapper.Map<Order>(input);
            return await Repository.InsertAsync(order);
            //  return await base.CreateAsync(input);
        }
    }
}
