using Abp.Application.Services.Dto;

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
}