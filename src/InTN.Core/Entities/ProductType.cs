using Abp.Domain.Entities;

namespace InTN.Entities
{
    public class ProductType : Entity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
