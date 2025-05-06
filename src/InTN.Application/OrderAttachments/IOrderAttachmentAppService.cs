using Abp.Application.Services.Dto;
using Abp.Application.Services;
using InTN.OrderAttachments.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.OrderAttachments
{
    public interface IOrderAttachmentAppService : IAsyncCrudAppService<OrderAttachmentDto, int, PagedResultRequestDto, OrderAttachmentDto, OrderAttachmentDto>
    {
        public Task<List<OrderAttachmentDto>> GetAttachmentsByOrderIdAsync(int orderId);
    }
}
