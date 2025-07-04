using Abp.Application.Services.Dto;

namespace InTN.Products.Dto
{
    public class ProductDto : EntityDto<int>
    {
        public string Code { get; set; } // Mã hàng
        public string Name { get; set; }
        public string Unit { get; set; } // Đơn vị tính
        public string Description { get; set; }
        public string InvoiceNote { get; set; }
        public string Properties { get; set; } // JSON string chứa các thuộc tính sản phẩm
        public int? ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } // Tên loại sản phẩm

        public int? ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; } // Tên danh mục sản phẩm


        public int? SupplierId { get; set; }
        public string SupplierName { get; set; } // Tên nhà cung cấp

        public int? BrandId { get; set; }
        public string BrandName { get; set; } // Tên thương hiệu

        public decimal? Price { get; set; }
        public decimal? Cost { get; set; }
        public string FileUploadIds { get; set; } // Danh sách ID của các tệp đính kèm
        public bool IsActive { get; set; }
    }
}