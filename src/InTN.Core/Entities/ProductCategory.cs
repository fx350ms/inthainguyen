﻿using Abp.Domain.Entities;

namespace InTN.Entities
{
    public class ProductCategory : Entity<int>
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }
}
