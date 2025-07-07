using Abp.Application.Services.Dto;

namespace InTN.Customers.Dto
{
    public class CustomerDto : EntityDto<int>
    {
        public string Name { get; set; } = string.Empty; // Tên khách hàng
        public string Email { get; set; } = string.Empty; // Email khách hàng
        public string PhoneNumber { get; set; } = string.Empty; // Số điện thoại
        public string Address { get; set; } = string.Empty; // Địa chỉ
        public decimal TotalDebt { get; set; } = 0.00m;
        public decimal? CreditLimit { get; set; } = 0.00m; // Giới hạn công nợ tối đa

        public int CustomerType { get; set; }   // Loại khách hàng (VD: "cá nhân", "doanh nghiệp")
        public string CustomerCode { get; set; } = string.Empty; // Mã khách hàng
        public string DeliveryArea { get; set; } = string.Empty; // Khu vực giao hàng
        public string Company { get; set; } = string.Empty; // Công ty
        public string TaxCode { get; set; } = string.Empty; // Mã số thuế
        public int Gender { get; set; } = 1; // Giới tính (1: Nam, 2: Nữ)
        public string Note { get; set; } = string.Empty; // Ghi chú
        public decimal? TotalOrderAmount { get; set; } = 0.00m; // Tổng số tiền đã đặt hàng
    }
}
