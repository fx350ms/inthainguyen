using Abp.Application.Services.Dto;
using Abp.Application.Services;
using InTN.OrderLogs.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.OrderLogs
{
    public interface IOrderLogAppService : IAsyncCrudAppService<OrderLogDto, int, PagedResultRequestDto, OrderLogDto, OrderLogDto>
    {
        public Task<List<OrderLogDto>> GetOrderLogsByOrderIdAsync(int orderId);
    }
}
