using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;

using InTN.OrderAttachments.Dto;
using InTN.Entities;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace InTN.OrderAttachments
{
    public class OrderAttachmentAppService : AsyncCrudAppService<OrderAttachment, OrderAttachmentDto, int, PagedResultRequestDto, OrderAttachmentDto, OrderAttachmentDto>, IOrderAttachmentAppService
    {
        public OrderAttachmentAppService(IRepository<OrderAttachment> repository)
            : base(repository)
        {
        }


        //public async Task<ComplaintDto> CreateWithImagesAsync([FromForm] ComplaintDto input)
        //{

        //    input.Status = (int)ComplaintStatus.Pending;

        //    var complaint = await base.CreateAsync(input);

        //    // Lưu hình ảnh
        //    if (input.Images != null && input.Images.Any())
        //    {
        //        foreach (var file in input.Images)
        //        {
        //            using (var memoryStream = new MemoryStream())
        //            {
        //                await file.CopyToAsync(memoryStream); // Đọc dữ liệu từ file

        //                var image = new ComplaintImageDto
        //                {
        //                    ComplaintId = complaint.Id,
        //                    ImageData = memoryStream.ToArray(), // Dữ liệu nhị phân của hình ảnh
        //                    ContentType = file.ContentType,    // Loại file (image/jpeg, image/png)
        //                    FileName = file.FileName           // Tên file
        //                };
        //                try
        //                {
        //                    await _complaintImageService.CreateAsync(image);
        //                }
        //                catch (System.Exception ex)
        //                {

        //                }
        //                // Lưu hình ảnh vào database

        //            }
        //        }
        //    }

        //    return complaint;
        //}
    }
}
