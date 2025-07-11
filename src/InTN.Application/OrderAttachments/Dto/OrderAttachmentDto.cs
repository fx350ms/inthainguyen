using Abp.Application.Services.Dto;

namespace InTN.OrderAttachments.Dto
{
    public class OrderAttachmentDto : EntityDto<int>
    {
        public int OrderId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public int FileId { get; set; }
        public int Type { get; set; }
    }
}
