using Abp.Domain.Entities;

namespace InTN.Entities
{
    public class Brand : Entity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
