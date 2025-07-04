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

        /// <summary>
        /// Ghi chú theo nhóm hàng
        /// </summary>
        public int ProductCategoryId { get; set; } 

        public string Note { get; set; } = string.Empty; // Nội dung ghi chú
         
    }
}
