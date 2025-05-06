using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;

using InTN.OrderAttachments.Dto;
using InTN.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InTN.OrderAttachments
{
    public class OrderAttachmentAppService : AsyncCrudAppService<OrderAttachment, OrderAttachmentDto, int, PagedResultRequestDto, OrderAttachmentDto, OrderAttachmentDto>, IOrderAttachmentAppService
    {
        public OrderAttachmentAppService(IRepository<OrderAttachment> repository)
            : base(repository)
        {
        }

        public async Task<List<OrderAttachmentDto>> GetAttachmentsByOrderIdAsync(int orderId)
        {
            var attachments = await Repository.GetAllListAsync(a => a.OrderId == orderId);
            return ObjectMapper.Map<List<OrderAttachmentDto>>(attachments);
        }
         
    }
}
