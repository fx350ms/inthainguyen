using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.FileUploads.Dto;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.FileUploads
{
    public interface IFileUploadAppService : IAsyncCrudAppService<
        FileUploadDto, // DTO chính
        int,           // Kiểu dữ liệu của khóa chính
        PagedResultRequestDto, // DTO cho phân trang
        CreateFileUploadDto,   // DTO cho tạo mới
        FileUploadDto>         // DTO cho cập nhật
    {
        Task DeleteWithAttachmentAsync(int id);
        Task<FileUploadDto> GetFileContentAsync(int id); // Lấy nội dung tệp
        Task<int> UploadFileAndGetIdsAsync(List<IFormFile> Attachments);
        Task<List<int>> UploadMultiFilesAndGetIdsAsync(List<IFormFile> Attachments); // Tạo mới tệp đính kèm
    }
}