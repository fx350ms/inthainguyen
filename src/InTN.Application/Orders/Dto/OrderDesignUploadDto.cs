using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace InTN.Orders.Dto
{
    public class OrderDesignUploadDto
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public List<IFormFile> Attachments { get; set; }
        
    }
}
