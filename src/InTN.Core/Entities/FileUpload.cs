using Abp.Domain.Entities;


namespace InTN.Entities
{
    public class FileUpload : Entity<int>
    {
        public string FileName { get; set; } = string.Empty; // Tên tệp đính kèm
        public string FileType { get; set; } = string.Empty; // Loại tệp (ví dụ: "image/png", "application/pdf")
        public long FileSize { get; set; } // Kích thước tệp (tính bằng byte)
        public byte[] FileContent { get; set; } // Nội dung tệp (dưới dạng mảng byte)
        public int Type { get; set; } // Loại tệp đính kèm (ví dụ: 1 cho hóa đơn, 2 cho chứng từ khác)
    }
}
