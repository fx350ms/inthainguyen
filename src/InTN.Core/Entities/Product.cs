using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTN.Entities
{
    public class Product :FullAuditedEntity<int>
    {
        // Thông tin cơ bản
        public string Code { get; set; } // Mã hàng
        public string Name { get; set; }
        // Đơn vị tính và thuộc tính
        public string Unit { get; set; } // Đơn vị tính (ví dụ: cái, kg, lít)
        // Mô tả
        public string Description { get; set; }
        public string InvoiceNote { get; set; }
        public string Properties { get; set; } // JSON string chứa các thuộc tính sản phẩm
        public int? ProductTypeId { get; set; } // Loại sản phẩm (ví dụ: hàng hóa, dịch vụ)
        public int? ProductCategoryId { get; set; } // Danh mục sản phẩm
        public int? SupplierId { get; set; } // Nhà cung cấp
        public int? BrandId { get; set; } // Thương hiệu
        public decimal? Price { get; set; } // Giá bán
        public decimal? Cost { get; set; } // Giá vốn
        public string FileUploadIds { get; set; } // Danh sách ID của các tệp đính kèm (dưới dạng chuỗi JSON hoặc danh sách ID)   
        public bool IsActive { get; set; } = true; // Trạng thái hoạt động của sản phẩm


        [AllowNull]
        [ForeignKey("ProductTypeId")]
        public virtual ProductType ProductType { get; set; } // Liên kết đến loại sản phẩm

        [AllowNull]
        [ForeignKey("ProductCategoryId")]
        public virtual ProductCategory ProductCategory { get; set; } // Liên kết đến danh mục sản phẩm

        [AllowNull]
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; } // Liên kết đến nhà cung cấp

        [AllowNull]
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; } // Liên kết đến thương hiệu


    }
}
