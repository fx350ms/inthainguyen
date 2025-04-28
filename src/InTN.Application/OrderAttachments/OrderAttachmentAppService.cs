using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;

using InTN.OrderAttachments.Dto;
using InTN.Entities;

namespace InTN.OrderAttachments
{
    public class OrderAttachmentAppService : AsyncCrudAppService<OrderAttachment, OrderAttachmentDto, int, PagedResultRequestDto, OrderAttachmentDto, OrderAttachmentDto>, IOrderAttachmentAppService
    {
        public OrderAttachmentAppService(IRepository<OrderAttachment> repository)
            : base(repository)
        {
        }
    }
}
