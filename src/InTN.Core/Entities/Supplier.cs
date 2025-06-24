using Abp.Domain.Entities;

namespace InTN.Entities
{
    public class Supplier : Entity<int>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
