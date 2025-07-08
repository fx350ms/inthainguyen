using Abp.Notifications;
using InTN.Commons;
using System;


namespace InTN.Orders.Dto
{
    public class OrderCreatedNotificationDto : NotificationData
    {
        public string CreatorName { get; set; }
        public string OrderCode { get; set; }
        public int OrderId { get; set; }
        public string Message { get; set; }


        public OrderCreatedNotificationDto()
        {
        }
        public OrderCreatedNotificationDto(string creatorName, string orderCode, int orderId, string message)
        {
            CreatorName = creatorName;
            OrderCode = orderCode;
            OrderId = orderId;
            Message = message ;
        }

        public override string Type => "OrderCreatedNotification";
    }
}
