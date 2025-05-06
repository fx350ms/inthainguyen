using Abp.Application.Services.Dto;

namespace InTN.OrderLogs.Dto
{
    public class OrderLogDto : FullAuditedEntityDto<int>
    {
        public int OrderId { get; set; }
        public string Action { get; set; } // Hành động (Created, Updated, Completed, etc.)
        public string Note { get; set; } // Ghi chú thêm về hành động
        public string OldValue { get; set; } // Giá trị cũ (nếu có)
        public string NewValue { get; set; } // Giá trị mới (nếu có)
    }
}
