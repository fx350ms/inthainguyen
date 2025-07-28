using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTN.Entities
{
    public class Printer : Entity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
