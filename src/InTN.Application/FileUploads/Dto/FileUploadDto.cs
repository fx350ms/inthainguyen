using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace InTN.FileUploads.Dto
{
    public class FileUploadDto : EntityDto<int>
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public int Type { get; set; }
    }

    public class CreateFileUploadDto
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public byte[] FileContent { get; set; }
        public int Type { get; set; }
    }

    public class UploadFileOnScriptDto
    {
        public byte[] FileContent { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}