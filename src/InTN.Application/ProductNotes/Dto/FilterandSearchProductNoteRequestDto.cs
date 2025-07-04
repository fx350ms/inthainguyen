using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTN.ProductNotes.Dto
{
    public class FilterandSearchProductNoteRequestDto
    {
        public string Keyword { get; set; } = string.Empty; // Từ khoá tìm kiếm
        public int? ProductCategoryId { get; set; }
    }
}
