using Abp.Domain.Entities;


namespace InTN.Entities
{
    public class OrderAttachment : Entity<int>
    {
        public int OrderId { get; set; }
        public string FileName { get; set; } = string.Empty; // Tên tệp đính kèm
        public string FileType { get; set; } = string.Empty; // Loại tệp (ví dụ: "image/png", "application/pdf")
        public long FileSize { get; set; } // Kích thước tệp (tính bằng byte)
        public int FileId { get; set; } // ID của tệp đính kèm trong hệ thống lưu trữ (ví dụ: ID của tệp trong hệ thống quản lý tệp hoặc dịch vụ lưu trữ đám mây)
        public int Type { get; set; } // Loại tệp đính kèm (ví dụ: 1 cho hóa đơn, 2 cho chứng từ khác)
    }
}
