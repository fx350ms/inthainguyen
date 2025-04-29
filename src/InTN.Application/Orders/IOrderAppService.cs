using Abp.Application.Services.Dto;
using Abp.Application.Services;
using InTN.Orders.Dto;
using InTN.Entities;
using System.Threading.Tasks;

namespace InTN.Orders
{
    public interface IOrderAppService : IAsyncCrudAppService<OrderDto, int, PagedResultRequestDto, OrderDto, OrderDto>
    {
        public Task<Order> CreateNewAsync(CreateOrderDto input);

      //  public Task CreateQuotation(QuotationDto input);
    }
}
