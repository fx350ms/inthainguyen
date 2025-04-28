using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;

using InTN.OrderLogs.Dto;
using InTN.Entities;

namespace InTN.OrderLogs
{
    public class OrderLogAppService : AsyncCrudAppService<OrderLog, OrderLogDto, int, PagedResultRequestDto, OrderLogDto, OrderLogDto>, IOrderLogAppService
    {
        public OrderLogAppService(IRepository<OrderLog> repository)
            : base(repository)
        {
        }
    }
}
