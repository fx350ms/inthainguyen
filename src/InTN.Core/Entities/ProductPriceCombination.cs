using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTN.Entities
{
    public class ProductPriceCombinationPrice : Entity<int>
    {
        public int ProductId { get; set; }
        public string PriceCombination { get; set; } // JSON string chứa các thuộc tính giá
    }
}
