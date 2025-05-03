using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTN.Orders.Dto
{
    public class QuotationDto
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public List<IFormFile> Attachments { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class OrderDesignUploadDto
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public List<IFormFile> Attachments { get; set; }
        
    }


    public class OrderDepositUploadDto
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public List<IFormFile> Attachments { get; set; }

    }
}
