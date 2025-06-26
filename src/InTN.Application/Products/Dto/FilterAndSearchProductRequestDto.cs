using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTN.Products.Dto
{
    public class FilterAndSearchProductRequestDto
    {
        public string Keyword { get; set; } = string.Empty; // Từ khoá tìm kiếm
        public int? ProductTypeId { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? SupplierId { get; set; }
        public int? BrandId { get; set; }
    }
}
