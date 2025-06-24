using Abp.Domain.Entities;

namespace InTN.Entities
{
    public class ProductProperty : Entity<int>
    {
        public string Name { get; set; }
    }
}
