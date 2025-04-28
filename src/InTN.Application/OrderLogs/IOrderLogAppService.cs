using Abp.Application.Services.Dto;
using Abp.Application.Services;
using InTN.OrderLogs.Dto;

namespace InTN.OrderLogs
{
    public interface IOrderLogAppService : IAsyncCrudAppService<OrderLogDto, int, PagedResultRequestDto, OrderLogDto, OrderLogDto>
    {
    }
}
