using InTN.OrderAttachments.Dto;
using InTN.OrderLogs.Dto;
using InTN.Orders.Dto;
using System.Collections.Generic;

namespace InTN.Web.Models.Orders
{
    public class OrderDetailModel
    {
        public OrderDto OrderDto { get; set; }
        public List<OrderDetailViewDto> OrderDetails { get; set; } = new List<OrderDetailViewDto>();
        public List<OrderAttachmentDto> OrderAttachments { get; set; }
        public List<OrderLogDto> OrderLogs { get; set; }
    }
}
