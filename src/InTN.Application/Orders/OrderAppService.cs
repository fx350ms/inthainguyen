using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;

using InTN.Orders.Dto;
using InTN.Entities;
using System.Threading.Tasks;

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
            return  await base.CreateAsync(input);
        }
    }
}
