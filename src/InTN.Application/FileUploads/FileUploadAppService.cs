using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.FileUploads.Dto;
using InTN.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InTN.FileUploads
{
    public class FileUploadAppService : AsyncCrudAppService<
        FileUpload, // Entity chính
        FileUploadDto, // DTO chính
        int,           // Kiểu dữ liệu của khóa chính
        PagedResultRequestDto, // DTO cho phân trang
        CreateFileUploadDto,   // DTO cho tạo mới
        FileUploadDto>,        // DTO cho cập nhật
        IFileUploadAppService  // Interface
    {

        private readonly IRepository<OrderAttachment, int> _orderAttachmentRepository;


        public FileUploadAppService(IRepository<FileUpload> repository,
            IRepository<OrderAttachment, int> orderAttachmentRepository)
            : base(repository)
        {
            _orderAttachmentRepository = orderAttachmentRepository;
        }

        public async Task<List<int>> UploadMultiFilesAndGetIdsAsync(List<IFormFile> Files)
        {
            var listIds = new List<int>();
            if (Files != null && Files.Count > 0)
            {
                foreach (var file in Files)
                {

                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream); // Đọc dữ liệu từ file

                        var attachment = new CreateFileUploadDto
                        {
                            FileName = file.FileName,
                            FileType = file.ContentType,    // Loại file (image/jpeg, image/png)
                            FileContent = memoryStream.ToArray(), // Dữ liệu nhị phân của hình ảnh
                            FileSize = file.Length,
                            Type = (int)FileUploadType.ProductImage,    // Loại file (image/jpeg, image/png)
                        };

                        try
                        {
                            var fileUpload = CreateAsync(attachment);
                            listIds.Add(fileUpload.Id); // Thêm ID của file đã upload vào danh sách
                        }
                        catch (System.Exception ex)
                        {
                            Logger.Error("Error uploading file: " + file.FileName, ex);
                        }
                    }
                }

            }

            return listIds;
        }


        public async Task<int> UploadFileAndGetIdsAsync(List<IFormFile> Files)
        {
            var listIds = new List<int>();
            if (Files != null && Files.Count > 0)
            {
                var file = Files[0];
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream); // Đọc dữ liệu từ file

                    var attachment = new CreateFileUploadDto
                    {
                        FileName = file.FileName,
                        FileType = file.ContentType,    // Loại file (image/jpeg, image/png)
                        FileContent = memoryStream.ToArray(), // Dữ liệu nhị phân của hình ảnh
                        FileSize = file.Length,
                        Type = (int)FileUploadType.ProductImage,    // Loại file (image/jpeg, image/png)
                    };

                    try
                    {
                        var fileUpload = CreateAsync(attachment);
                        return fileUpload.Id; // Thêm ID của file đã upload vào danh sách
                    }
                    catch (System.Exception ex)
                    {
                        Logger.Error("Error uploading file: " + file.FileName, ex);
                    }
                }
            }

            return 0; // Trả về 0 nếu không có file nào được upload
        }


        //public async Task<FileUploadDto> UploadAndGetInfoAsync(List<IFormFile> Attachments)
        //{
        //    if (Attachments == null || Attachments.Count == 0)
        //    {
        //        return null; // Trả về null nếu không có file nào được upload
        //    }
        //    var file = Attachments[0];
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        await file.CopyToAsync(memoryStream); // Đọc dữ liệu từ file
        //        var attachment = new CreateFileUploadDto
        //        {
        //            FileName = file.FileName,
        //            FileType = file.ContentType,    // Loại file (image/jpeg, image/png)
        //            FileContent = memoryStream.ToArray(), // Dữ liệu nhị phân của hình ảnh
        //            FileSize = file.Length,
        //            Type = (int)FileUploadType.ProductImage,    // Loại file (image/jpeg, image/png)
        //        };
        //        try
        //        {
        //            var fileUpload = await CreateAsync(attachment);
        //            return ObjectMapper.Map<FileUploadDto>(fileUpload); // Trả về thông tin của file đã upload
        //        }
        //        catch (System.Exception ex)
        //        {
        //            Logger.Error("Error uploading file: " + file.FileName, ex);
        //            return null; // Trả về null nếu có lỗi xảy ra
        //        }
        //    }
        //}

        [HttpPost]
        public async Task<FileUploadDto> UploadAndGetInfoAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null; // Trả về null nếu không có file nào được upload
            }

            return null;
        }


        public async Task<FileUploadDto> GetFileContentAsync(int id)
        {
            var fileUpload = await Repository.GetAsync(id);
            var fileUploadDto = ObjectMapper.Map<FileUploadDto>(fileUpload);
            return fileUploadDto;
        }


        [HttpPost]
        public async Task<List<int>> UploadFilesAndGetIdsAsync([FromForm] List<IFormFile> files)
        {
            var listIds = new List<int>();
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
                            var fileUploadId = await Repository.InsertAndGetIdAsync(attachment);
                            listIds.Add(fileUploadId); // Thêm ID của file đã upload vào danh sách
                        }
                        catch (System.Exception ex)
                        {
                            Logger.Error("Error uploading file: " + file.FileName, ex);
                        }
                    }
                }

            }

            return listIds; // Trả về 0 nếu không có file nào được upload
        }


        [HttpPost]
        public async Task<int> UploadFileAndGetIdAsync([FromForm] List<IFormFile> files)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];

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
                        var fileUploadId = await Repository.InsertAndGetIdAsync(attachment);
                        return fileUploadId; // Trả về ID của file đã upload
                    }
                    catch (System.Exception ex)
                    {
                        Logger.Error("Error uploading file: " + file.FileName, ex);
                    }
                }
            }
            return 0; // Trả về 0 nếu không có file nào được upload
        }


        public async Task<List<FileUploadDto>> GeAttachmentstByOrderIdAsync(int orderId, int fileType)
        {
            var attachments = (await _orderAttachmentRepository.GetAllAsync())
                .Where(a => a.OrderId == orderId && (fileType == -1 || a.Type == fileType)).ToList();
            var fileUploadIds = attachments.Select(a => a.FileId).ToList();
            var files = await Repository.GetAllListAsync(f => fileUploadIds.Contains(f.Id));
            return ObjectMapper.Map<List<FileUploadDto>>(files);
          
        }

        public async Task DeleteWithAttachmentAsync(int id)
        {
            var attachment = await _orderAttachmentRepository.FirstOrDefaultAsync(u => u.FileId == id);
            if(attachment != null)
            {
                await _orderAttachmentRepository.DeleteAsync(attachment);
            }   
            var file = await Repository.GetAsync(id);
            if (file != null)
            {
                await Repository.DeleteAsync(file);
            }
        }
    }
}