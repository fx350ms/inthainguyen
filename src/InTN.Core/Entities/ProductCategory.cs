using Abp.Domain.Entities;

namespace InTN.Entities
{
    public class ProductCategory : Entity<int>
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }

    public class ProductProperty : Entity<int>
    {
        public string Name { get; set; }
    }

    public class Supplier : Entity<int>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public class Brand : Entity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ProductType : Entity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
