using Abp.Application.Services.Dto;
using System;

namespace InTN.Orders.Dto
{
    public class CreateOrderDto : EntityDto<int>
    {
        public bool IsCasualCustomer { get; set; } = false; // true: Khách hàng vãng lai, false: Khách hàng đã có trong hệ thống
        public string OrderCode { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Status { get; set; }
        public string Note { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty; // Đường dẫn đến file đính kèm

        public string CustomerName { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
    }
}
