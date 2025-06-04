using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.FileUploads.Dto;
using InTN.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;

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
        private readonly IRepository<FileUpload> _fileUploadRepository;

        public FileUploadAppService(IRepository<FileUpload> repository) : base(repository)
        {
            _fileUploadRepository = repository;
        }

        public async Task<List<int>> UploadMultiFilesAndGetIdsAsync(List<IFormFile> Attachments)
        {
            var listIds = new List<int>();  
            if (Attachments != null && Attachments.Count > 0)
            {
                foreach (var file in Attachments)
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

        public async Task<FileUploadDto> GetFileContentAsync(int id)
        {
            var fileUpload = await _fileUploadRepository.GetAsync(id);
            var fileUploadDto = ObjectMapper.Map<FileUploadDto>(fileUpload);
            return fileUploadDto;
        }
    }
}