using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;

using InTN.OrderLogs.Dto;
using InTN.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.OrderLogs
{
    public class OrderLogAppService : AsyncCrudAppService<OrderLog, OrderLogDto, int, PagedResultRequestDto, OrderLogDto, OrderLogDto>, IOrderLogAppService
    {
        public OrderLogAppService(IRepository<OrderLog> repository)
            : base(repository)
        {
        }

        public async Task<List<OrderLogDto>> GetOrderLogsByOrderIdAsync(int orderId)
        {
            var orderLogs = await Repository.GetAllListAsync(log => log.OrderId == orderId);
            return ObjectMapper.Map<List<OrderLogDto>>(orderLogs);
        }
    }
}
