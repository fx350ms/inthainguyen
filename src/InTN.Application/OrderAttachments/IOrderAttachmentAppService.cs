using Abp.Application.Services.Dto;
using Abp.Application.Services;
using InTN.OrderAttachments.Dto;

namespace InTN.OrderAttachments
{
    public interface IOrderAttachmentAppService : IAsyncCrudAppService<OrderAttachmentDto, int, PagedResultRequestDto, OrderAttachmentDto, OrderAttachmentDto>
    {
    }
}
