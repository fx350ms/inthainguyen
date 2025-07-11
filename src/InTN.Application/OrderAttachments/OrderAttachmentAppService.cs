using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;

using InTN.OrderAttachments.Dto;
using InTN.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace InTN.OrderAttachments
{
    public class OrderAttachmentAppService : AsyncCrudAppService<OrderAttachment, OrderAttachmentDto, int, PagedResultRequestDto, OrderAttachmentDto, OrderAttachmentDto>, IOrderAttachmentAppService
    {

        private readonly IRepository<FileUpload> _fileUploadRepository;
        public OrderAttachmentAppService(IRepository<OrderAttachment> repository,
            IRepository<FileUpload> fileUploadRepository)
            : base(repository)
        {
            _fileUploadRepository = fileUploadRepository;
        }

        public async Task<List<OrderAttachmentDto>> GetAttachmentsByOrderIdAsync(int orderId)
        {
            var attachments = await Repository.GetAllListAsync(a => a.OrderId == orderId);
            return ObjectMapper.Map<List<OrderAttachmentDto>>(attachments);
        }



        [HttpPost]
        public async Task<List<object>> UploadAttachmentsAsync([FromForm] List<IFormFile> files, int orderId, int attachmentType)
        {
            var result = new List<object>();
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream); // Đọc dữ liệu từ file

                        var attachment = new FileUpload
                        {
                            FileName = file.FileName,
                            FileType = file.ContentType,    // Loại file (image/jpeg, image/png)
                            FileContent = memoryStream.ToArray(), // Dữ liệu nhị phân của hình ảnh
                            FileSize = file.Length
                        };

                        try
                        {
                            var fileUploadId = await _fileUploadRepository.InsertAndGetIdAsync(attachment);

                            var orderAttachment = new OrderAttachment
                            {
                                OrderId = orderId,
                                FileName = file.FileName,
                                FileType = file.ContentType,
                                FileSize = file.Length,
                                FileId = fileUploadId, // Lưu ID của file đã upload
                                Type = attachmentType // Loại tệp đính kèm (ví dụ: 1 cho hóa đơn, 2 cho chứng từ khác)
                            };
                            await Repository.InsertAsync(orderAttachment); // Lưu thông tin đính kèm vào cơ sở dữ liệu

                            result.Add(new
                            {
                                Success = true,
                                FileUploadId = fileUploadId,
                                FileName = file.FileName,
                            }); // Thêm ID của file đã upload vào danh sách
                        }
                        catch (System.Exception ex)
                        {
                            result.Add(new
                            {
                                Success = false,
                                Message = "Error uploading file: " + file.FileName,
                            });
                            Logger.Error("Error uploading file: " + file.FileName, ex);
                        }
                    }
                }

            }

            return result; // Trả về 0 nếu không có file nào được upload
        }


    }
}
