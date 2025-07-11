using Abp.Application.Services.Dto;
using Abp.Application.Services;
using InTN.OrderAttachments.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace InTN.OrderAttachments
{
    public interface IOrderAttachmentAppService : IAsyncCrudAppService<OrderAttachmentDto, int, PagedResultRequestDto, OrderAttachmentDto, OrderAttachmentDto>
    {
        public Task<List<OrderAttachmentDto>> GetAttachmentsByOrderIdAsync(int orderId);
        Task<List<object>> UploadAttachmentsAsync([FromForm] List<IFormFile> files, int orderId, int attachmentType);
    }
}
