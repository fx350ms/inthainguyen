using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace InTN.Orders.Dto
{
    public class OrderTransactionUploadDto
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public List<IFormFile> Attachments { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDeposit { get; set; }
        public decimal Amount { get; set; }
        public int PaymentStatus { get; set; }
    }
}
