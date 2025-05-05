using Abp.Domain.Entities;

namespace InTN.Entities
{
    public class Customer : Entity<int>
    {
        public string Name { get; set; } = string.Empty; // Tên khách hàng
        public string Email { get; set; } = string.Empty; // Email khách hàng
        public string PhoneNumber { get; set; } = string.Empty; // Số điện thoại
        public string Address { get; set; } = string.Empty; // Địa chỉ

        public decimal TotalDebt { get; set; } = 0.00m;
        public decimal? CreditLimit { get; set; } = 0.00m; // Giới hạn công nợ tối đa
    }
}
