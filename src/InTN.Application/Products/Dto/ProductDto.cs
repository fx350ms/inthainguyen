using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

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
        public int? ProductCategoryId { get; set; }
        public int? SupplierId { get; set; }
        public int? BrandId { get; set; }
        public decimal? Price { get; set; }
        public decimal? Cost { get; set; }
        public string FileUploadIds { get; set; } // Danh sách ID của các tệp đính kèm
        public bool IsActive { get; set; }
    }

    public class ProductWithImageDto : EntityDto<int>
    {
        public string Code { get; set; } // Mã hàng
        public string Name { get; set; }
        public string Unit { get; set; } // Đơn vị tính
        public string Description { get; set; }
        public string InvoiceNote { get; set; }
        public string Properties { get; set; } // JSON string chứa các thuộc tính sản phẩm
        public int? ProductTypeId { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? SupplierId { get; set; }
        public int? BrandId { get; set; }
        public decimal? Price { get; set; }
        public decimal? Cost { get; set; }
        
        public string FileUploadIds { get; set; } // Danh sách ID của các tệp đính kèm
        public bool IsActive { get; set; }
    }

    public class CreateProductDto : EntityDto<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public string InvoiceNote { get; set; }
        public string Properties { get; set; }
        public int? ProductTypeId { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? SupplierId { get; set; }
        public int? BrandId { get; set; }
        public decimal? Price { get; set; }
        public decimal? Cost { get; set; }
        public string FileUploadIds { get; set; }
        public bool IsActive { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}