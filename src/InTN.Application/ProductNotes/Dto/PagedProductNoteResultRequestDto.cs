using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTN.ProductNotes.Dto
{
    public class PagedProductNoteResultRequestDto : PagedResultRequestDto
    {
        public int? ProductId { get; set; }
        public int? ParentId{ get; set; }
        public string Keyword { get; set; } = string.Empty; // Từ khóa tìm kiếm trong ghi chú
    }
}
