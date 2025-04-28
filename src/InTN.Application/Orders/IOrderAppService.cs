using Abp.Application.Services.Dto;
using Abp.Application.Services;
using InTN.Orders.Dto;

namespace InTN.Orders
{
    public interface IOrderAppService : IAsyncCrudAppService<OrderDto, int, PagedResultRequestDto, OrderDto, OrderDto>
    {
    }
}
