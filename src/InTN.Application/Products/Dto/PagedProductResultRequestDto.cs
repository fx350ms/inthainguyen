using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTN.Products.Dto
{
    public class PagedProductResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; } // Mã hàng
        public int? ProductTypeId { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? SupplierId { get; set; }
        public int? BrandId { get; set; }
        public decimal? Price { get; set; }
        public decimal? Cost { get; set; }
        public int Status { get; set; }
    }
}
