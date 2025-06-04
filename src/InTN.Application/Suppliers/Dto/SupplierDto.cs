using Abp.Application.Services.Dto;

namespace InTN.Suppliers.Dto
{
    public class SupplierDto : EntityDto<int>
    {
        public string Name { get; set; } // Tên nhà cung cấp
        public string Address { get; set; } // Địa chỉ
        public string Phone { get; set; } // Số điện thoại
        public string Email { get; set; } // Email
    }
}