using System.ComponentModel;

namespace InTN;

public enum OrderStatus
{
    [Description("Tiếp nhận yêu cầu")]
    ReceivedRequest = 1, // Tiếp nhận yêu cầu  

    [Description("Đã báo giá")]
    Quoted = 2, // Đã báo giá  

    [Description("Đã xác nhận đơn")]
    OrderConfirmed = 3, // Đã xác nhận đơn  

    [Description("Đang thiết kế")]
    Designing = 4, // Đang thiết kế  

    [Description("Đang chờ duyệt mẫu")]
    AwaitingSampleApproval = 5, // Đang chờ duyệt mẫu  

    [Description("Đã duyệt mẫu")]
    DesignApproved = 6, // Đã duyệt mẫu  

    [Description("Đã đặt cọc")]
    Deposited = 7, // Đã đặt cọc  

    [Description("Đang in test")]
    PrintingTest = 8, // Đang in test  

    [Description("Xác nhận in test (Ok)")]
    PrintingTestConfirmed = 9, // Xác nhận in test (Ok)  

    [Description("Đang in")]
    Printing = 10, // Đang in  

    [Description("Đang gia công")]
    Processing = 11, // Đang gia công  

    [Description("Đã kiểm tra QC")]
    QcChecked = 12, // Đã kiểm tra QC  

    [Description("Đang giao hàng")]
    Delivering = 13, // Đang giao hàng  

    [Description("Hoàn thành nghiệm thu")]
    Completed = 14 // Hoàn thành nghiệm thu  
}


public enum OrderAttachmentType
{
    [Description("Khác")]
    OtherDocument = 1, // Chứng từ khác  

    [Description("Hóa đơn/Báo giá")]
    Invoice = 2, // Hóa đơn  

    [Description("Mẫu thiết kế")]
    DesignSample = 3, // Mẫu thiết kế


}
