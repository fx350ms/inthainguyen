# InTN - Database Schema và Data Models Documentation

## Database Overview

Hệ thống InTN sử dụng SQL Server làm database chính với Entity Framework Core làm ORM. Database được thiết kế theo Domain-Driven Design principles với các aggregate roots và value objects rõ ràng.

## Core Entities và Relationships

### 1. Product Management Domain

#### Product Entity
```csharp
public class Product : FullAuditedEntity<int>
{
    public string Code { get; set; }           // Mã hàng (unique)
    public string Name { get; set; }           // Tên sản phẩm
    public string Unit { get; set; }           // Đơn vị tính (cái, kg, lít)
    public string Description { get; set; }    // Mô tả chi tiết
    public string InvoiceNote { get; set; }    // Ghi chú hóa đơn
    public string Properties { get; set; }     // JSON string cho properties
    
    // Foreign Keys
    public int? ProductTypeId { get; set; }
    public int? ProductCategoryId { get; set; }
    public int? SupplierId { get; set; }
    public int? BrandId { get; set; }
    
    // Pricing
    public decimal? Price { get; set; }        // Giá bán
    public decimal? Cost { get; set; }         // Giá vốn
    
    // File Management
    public string FileUploadIds { get; set; }  // JSON array của file IDs
    
    public bool IsActive { get; set; } = true;
    
    // Navigation Properties
    public virtual ProductType ProductType { get; set; }
    public virtual ProductCategory ProductCategory { get; set; }
    public virtual Supplier Supplier { get; set; }
    public virtual Brand Brand { get; set; }
}
```

#### Related Product Entities
- **ProductCategory**: Phân loại sản phẩm hierarchical
- **ProductType**: Loại sản phẩm (hàng hóa, dịch vụ)
- **ProductProperty**: Định nghĩa thuộc tính động cho products
- **ProductPriceCombination**: Pricing matrix dựa trên property combinations
- **ProductNote**: Ghi chú kỹ thuật cho products

### 2. Order Management Domain

#### Order Entity (Aggregate Root)
```csharp
public class Order : FullAuditedEntity<int>
{
    public string OrderCode { get; set; }      // Mã đơn hàng (auto-generated)
    public int? CustomerId { get; set; }       // Foreign key to Customer
    public DateTime OrderDate { get; set; }
    
    // Status Management
    public int Status { get; set; }            // OrderStatus enum
    public int PaymentStatus { get; set; }     // PaymentStatus enum
    
    // Customer Information (denormalized for performance)
    public string CustomerName { get; set; }
    public string CustomerAddress { get; set; }
    public string CustomerPhone { get; set; }
    public string CustomerEmail { get; set; }
    public int CustomerGender { get; set; }    // 1: Anh, 2: Chị
    public string CustomerType { get; set; }   // Khách hàng, Nội bộ
    
    // Delivery Information
    public int DeliveryMethod { get; set; }    // ShippingMethod enum
    public DateTime? ExpectedDeliveryDate { get; set; }
    
    // Requirements Flags
    public bool IsRequireDesign { get; set; }
    public bool IsRequireTestSample { get; set; }
    public bool IsExportInvoice { get; set; }
    public bool IsStoreSample { get; set; }
    public bool IsReceiveByOthers { get; set; }
    public string OtherRequirements { get; set; }
    
    // Financial Information
    public decimal? TotalProductAmount { get; set; }
    public decimal? TotalDeposit { get; set; }
    public decimal? DeliveryFee { get; set; }
    public decimal? VatRate { get; set; }
    public decimal? VatAmount { get; set; }
    public decimal? TotalAmount { get; set; }
    
    // File Management
    public string FileIds { get; set; }        // JSON array của attached files
    
    public string Note { get; set; }
    
    // Navigation Properties
    public List<OrderDetail> OrderDetails { get; set; }
    public virtual Customer Customer { get; set; }
}
```

#### OrderDetail Entity
```csharp
public class OrderDetail : FullAuditedEntity<int>
{
    public int OrderId { get; set; }           // Foreign key
    public int? ProductId { get; set; }        // Foreign key (nullable for custom items)
    
    // Product Information (denormalized)
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public string ProductProperties { get; set; } // JSON properties at time of order
    
    // Quantity and Pricing
    public decimal Quantity { get; set; }
    public string Unit { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    
    public string Note { get; set; }
    
    // Navigation Properties
    public virtual Order Order { get; set; }
    public virtual Product Product { get; set; }
}
```

### 3. Customer Management Domain

#### Customer Entity
```csharp
public class Customer : Entity<int>
{
    // Basic Information
    public string CustomerCode { get; set; }   // Unique customer code
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    
    // Business Information
    public string Company { get; set; }
    public string TaxCode { get; set; }        // Mã số thuế
    public int CustomerType { get; set; }      // 1: Cá nhân, 2: Công ty
    public int Gender { get; set; }            // 1: Nam, 2: Nữ
    
    // Financial Information
    public decimal TotalDebt { get; set; } = 0.00m;
    public decimal? CreditLimit { get; set; } = 0.00m;
    public decimal? TotalOrderAmount { get; set; } = 0.00m;
    
    // Geographic Information
    public string DeliveryArea { get; set; }   // Khu vực giao hàng
    
    public string Note { get; set; }
}
```

### 4. Financial Management Domain

#### Transaction Entity
```csharp
public class Transaction : FullAuditedEntity<int>
{
    public string TransactionCode { get; set; }
    public int? CustomerId { get; set; }
    public int? OrderId { get; set; }
    
    public int TransactionType { get; set; }   // TransactionType enum
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    
    public string Description { get; set; }
    public string Note { get; set; }
    
    // Navigation Properties
    public virtual Customer Customer { get; set; }
    public virtual Order Order { get; set; }
}
```

#### CustomerBalanceHistory Entity
```csharp
public class CustomerBalanceHistory : FullAuditedEntity<int>
{
    public int CustomerId { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal ChangeAmount { get; set; }
    public decimal NewBalance { get; set; }
    public int DebtType { get; set; }          // Increase/Decrease
    public string Description { get; set; }
    
    public virtual Customer Customer { get; set; }
}
```

### 5. File Management Domain

#### FileUpload Entity
```csharp
public class FileUpload : FullAuditedEntity<int>
{
    public string FileName { get; set; }
    public string OriginalFileName { get; set; }
    public string FilePath { get; set; }
    public string ContentType { get; set; }
    public long FileSize { get; set; }
    
    public int FileType { get; set; }          // FileType enum
    public int FileUploadType { get; set; }    // FileUploadType enum
    
    // Polymorphic associations
    public string EntityType { get; set; }     // Product, Order, etc.
    public int? EntityId { get; set; }
    
    public string Description { get; set; }
}
```

### 6. Process Management Domain

#### Process và ProcessStep Entities
```csharp
public class Process : FullAuditedEntity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    
    public List<ProcessStep> ProcessSteps { get; set; }
}

public class ProcessStep : FullAuditedEntity<int>
{
    public int ProcessId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }             // Step sequence
    public bool IsActive { get; set; }
    
    public virtual Process Process { get; set; }
}
```

## Enums và Constants

### OrderStatus Enum
```csharp
public enum OrderStatus
{
    RequestReceived = 1,          // Tiếp nhận yêu cầu
    Quoted = 2,                   // Đã báo giá
    OrderConfirmed = 3,           // Đã xác nhận đơn
    Designing = 4,                // Đang thiết kế
    WaitingDesignApproval = 5,    // Đang chờ duyệt mẫu
    DesignApproved = 6,           // Đã duyệt mẫu
    Deposited = 7,                // Đã đặt cọc
    PrintingTest = 8,             // Đang in test
    PrintingTestConfirmed = 9,    // Xác nhận in test (Ok)
    Printing = 10,                // Đang in
    Processing = 11,              // Đang gia công
    QcChecked = 12,               // Đã kiểm tra QC
    Delivering = 13,              // Đang giao hàng
    Completed = 14                // Hoàn thành nghiệm thu
}
```

### PaymentStatus Enum
```csharp
public enum PaymentStatus
{
    Unpaid = 0,                   // Chưa thanh toán
    Deposited = 1,                // Đã đặt cọc
    FullyPaid = 2,                // Đã thanh toán
    DebtTransferred = 3           // Đã chuyển công nợ
}
```

### ShippingMethod Enum
```csharp
public enum ShippingMethod
{
    CompanyDelivery = 1,          // Công ty giao
    CustomerPickup = 2,           // Khách lấy
    ServiceDelivery = 3           // Giao hàng dịch vụ
}
```

## Database Relationships Summary

### One-to-Many Relationships
- Customer → Orders
- Order → OrderDetails
- Product → OrderDetails
- Customer → Transactions
- Customer → CustomerBalanceHistory
- ProductCategory → Products
- ProductType → Products
- Supplier → Products
- Brand → Products

### Many-to-Many Relationships
- Orders ↔ FileUploads (through FileIds JSON field)
- Products ↔ FileUploads (through FileUploadIds JSON field)

### Polymorphic Relationships
- FileUpload có thể associate với multiple entity types through EntityType/EntityId

## Indexing Strategy

### Primary Indexes
- All primary keys có clustered indexes
- Foreign keys có non-clustered indexes

### Performance Indexes
- Order.OrderCode (unique, frequently searched)
- Customer.CustomerCode (unique)
- Product.Code (unique)
- Order.Status, Order.OrderDate (composite for filtering)
- Customer.Name, Customer.PhoneNumber (for search)

### Audit Indexes
- CreationTime, LastModificationTime trên tất cả audited entities
- CreatorUserId, LastModifierUserId for audit trails

## Data Consistency Rules

### Business Rules Enforced
1. **Order Code Generation**: Auto-generated, unique, sequential
2. **Customer Balance**: Automatically calculated from transactions
3. **Order Total**: Sum of OrderDetail amounts + fees
4. **Status Transitions**: Enforced workflow rules
5. **File Associations**: Orphaned files cleaned up regularly

### Referential Integrity
- Soft deletes for most entities (IsDeleted flag)
- Foreign key constraints với appropriate cascade rules
- JSON field validation for structured data

## Performance Considerations

### Query Optimization
- Denormalized customer data in orders for performance
- Product properties stored as JSON for flexibility
- Indexed views for complex reporting queries

### Scalability Patterns
- Read replicas for reporting workloads
- Partitioning strategy for large tables (Orders, Transactions)
- Archiving strategy for completed orders

### Caching Strategy
- Customer lookup data
- Product catalog information
- Configuration settings
- User permissions

## Migration Strategy

### Schema Evolution
- Entity Framework Code-First migrations
- Backward compatibility maintained
- Data migration scripts for complex changes

### Deployment Process
1. Backup current database
2. Apply schema migrations
3. Run data migration scripts
4. Verify data integrity
5. Update application

## Security Considerations

### Data Protection
- Sensitive customer data encryption
- Audit trail for all data changes
- Row-level security for multi-tenant scenarios

### Access Control
- Database user permissions
- Application-level authorization
- API rate limiting
- SQL injection prevention