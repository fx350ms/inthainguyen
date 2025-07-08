# InTN - Technology Stack và Architecture Overview

## Tổng quan về dự án
InTN là hệ thống quản lý đơn hàng sản xuất in ấn và gia công được xây dựng trên nền tảng ASP.NET Core với Clean Architecture.

## Technology Stack

### Backend Technologies
- **.NET 9.0**: Framework chính cho backend development
- **ASP.NET Core MVC**: Web framework cho xây dựng ứng dụng web
- **ASP.NET Boilerplate (ABP)**: Framework để xây dựng ứng dụng enterprise với các tính năng:
  - Authentication & Authorization 
  - Multi-tenancy support
  - Audit logging
  - Setting management
  - Localization
- **Entity Framework Core**: ORM cho data access layer
- **SQL Server**: Hệ quản trị cơ sở dữ liệu
- **AutoMapper**: Object-to-object mapping
- **SignalR**: Real-time communication

### Frontend Technologies
- **HTML5/CSS3**: Markup và styling
- **JavaScript/jQuery**: Client-side scripting
- **Bootstrap**: CSS framework cho responsive design
- **AdminLTE**: Admin dashboard theme
- **DataTables**: Advanced table plugin cho jQuery
- **Select2**: Enhanced select controls
- **Dropzone.js**: File upload library
- **FamFamFam Icons**: Icon library

### Development Tools & Utilities
- **Castle Windsor**: Dependency injection container
- **Log4Net**: Logging framework
- **EPPlus**: Excel file processing (cho import/export)

## Architecture Pattern

Dự án tuân theo **Clean Architecture** với các layer:

### 1. Core Layer (`InTN.Core`)
- **Entities**: Domain models và business entities
- **Enums**: Các enum định nghĩa constants
- **Interfaces**: Contracts cho repositories và services
- **Value Objects**: Objects không có identity

**Key Entities:**
- `Product`: Sản phẩm với properties, pricing, categories
- `Order`: Đơn hàng với workflow status tracking
- `Customer`: Thông tin khách hàng và credit management
- `OrderDetail`: Chi tiết sản phẩm trong đơn hàng
- `Transaction`: Giao dịch thanh toán
- `FileUpload`: Quản lý file đính kèm

### 2. Application Layer (`InTN.Application`)
- **Application Services**: Business logic implementation
- **DTOs**: Data Transfer Objects
- **AutoMapper Profiles**: Object mapping configurations
- **Authorization**: Permission definitions

**Key Services:**
- `ProductAppService`: Quản lý sản phẩm, search, filter
- `OrderAppService`: Xử lý workflow đơn hàng
- `CustomerAppService`: Quản lý khách hàng
- `FileUploadAppService`: Xử lý upload files

### 3. Infrastructure Layer (`InTN.EntityFrameworkCore`)
- **DbContext**: Entity Framework configuration
- **Migrations**: Database schema changes
- **Repository Implementations**: Data access patterns

### 4. Web Layer (`InTN.Web.Mvc`, `InTN.Web.Core`, `InTN.Web.Host`)
- **Controllers**: HTTP request handlers
- **Views**: Razor templates
- **ViewModels**: Data models cho views
- **Static Files**: CSS, JS, images
- **Configuration**: App settings, authentication

### 5. Test Layer
- **Unit Tests**: Test business logic
- **Integration Tests**: Test end-to-end scenarios

## Business Domain

### Core Business Functions

#### 1. Product Management
- Product catalog với categories, properties, pricing
- Multiple pricing combinations based on properties
- File attachments for product images/specs
- Excel import functionality cho bulk product data

#### 2. Order Management Workflow
Hệ thống theo dõi đơn hàng qua các trạng thái:
1. **Báo giá** (Quotation)
2. **Đã duyệt báo giá** (Approved Quotation)
3. **Đã duyệt mẫu** (Design Approved)
4. **Đã đặt cọc** (Deposited)
5. **Đang in test** (Printing Test)
6. **Xác nhận in test OK** (Test Print Confirmed)
7. **Đang in** (Printing)
8. **Đang gia công** (Processing)
9. **Đã kiểm tra QC** (QC Checked)
10. **Đang giao hàng** (Delivering)
11. **Hoàn thành nghiệm thu** (Completed)

#### 3. Customer Relationship Management
- Customer profiles với contact information
- Credit limit và debt tracking
- Transaction history
- Multiple delivery addresses

#### 4. Financial Management
- Order payment tracking
- Deposit management
- VAT calculations
- Customer balance history

#### 5. File Management
- Upload và attach files to orders/products
- Support multiple file types
- File organization by type và purpose

## Key Features

### 1. Multi-language Support
- Vietnamese as primary language
- Localization infrastructure sẵn sàng cho expansion

### 2. Role-based Security
- ABP framework cung cấp authentication/authorization
- Permission-based access control
- User role management

### 3. Audit Trail
- Full audit logging cho tất cả entities
- Track creation, modification, deletion
- User activity monitoring

### 4. Real-time Updates
- SignalR integration cho real-time notifications
- Live updates cho order status changes

### 5. Responsive Design
- Mobile-friendly interface
- Bootstrap responsive grid system
- Adaptive layouts

## Database Schema Highlights

### Core Tables
- **Products**: Product catalog
- **ProductCategories**: Category hierarchy
- **ProductProperties**: Configurable product attributes
- **Orders**: Order headers
- **OrderDetails**: Line items
- **Customers**: Customer master data
- **Transactions**: Payment records
- **FileUploads**: File metadata

### Relationship Patterns
- **One-to-Many**: Customer → Orders, Order → OrderDetails
- **Many-to-Many**: Products ↔ Categories (through properties)
- **Polymorphic**: File attachments có thể link đến multiple entity types

## Development Workflow

### Build và Development
```bash
# Restore packages
dotnet restore

# Build solution  
dotnet build

# Run migrations
dotnet ef database update

# Run application
dotnet run --project src/InTN.Web.Mvc
```

### Testing
```bash
# Run unit tests
dotnet test test/InTN.Tests

# Run web tests  
dotnet test test/InTN.Web.Tests
```

## Configuration

### Connection Strings
- SQL Server database connection trong `appsettings.json`
- Separate settings cho Development/Production

### Key Settings
- Authentication configuration
- File upload paths và size limits
- Email service configuration
- Logging levels

## Security Considerations

### Authentication
- Cookie-based authentication
- JWT token support
- Session management

### Authorization
- Permission-based access control
- Feature-level authorization
- Data-level security

### Data Protection
- Input validation
- XSS protection
- CSRF protection via anti-forgery tokens

## Performance Optimizations

### Database
- Entity Framework change tracking optimization
- Lazy loading configurations
- Query optimization

### Frontend
- Bundling và minification
- CDN integration ready
- Client-side caching

### Caching Strategy
- ABP framework caching infrastructure
- Memory caching cho frequently accessed data

## Deployment Architecture

### Development Environment
- Local SQL Server/LocalDB
- IIS Express hosting
- File storage trên local filesystem

### Production Considerations
- SQL Server clustering
- Load balancer support
- Distributed file storage
- Application insights integration

## Future Expansion Possibilities

### Technical Enhancements
- API development cho mobile apps
- Microservices decomposition
- Docker containerization
- Cloud deployment (Azure/AWS)

### Business Features
- Advanced reporting và analytics
- Integration với external systems
- Mobile app development
- E-commerce frontend integration