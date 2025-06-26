using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTN.Entities
{
    public class ProductNote : Entity<int>
    {
        public int? ParentId { get; set; }
        public int ProductId { get; set; }
        public string Note { get; set; } = string.Empty; // Nội dung ghi chú

        //[AllowNull]
        //[ForeignKey("ParentId")]
        //public virtual ProductNote Parent { get; set; } // Ghi chú cha (nếu có)
    }
}
