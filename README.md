# InTN - Print Production Management System

## Tổng quan về dự án

InTN là hệ thống quản lý quy trình sản xuất in ấn và gia công được xây dựng trên nền tảng .NET với Clean Architecture. Hệ thống cung cấp giải pháp toàn diện cho việc quản lý đơn hàng, sản phẩm, khách hàng và quy trình sản xuất trong ngành in ấn.

## Tính năng chính

### 🏭 Quản lý quy trình sản xuất
- Theo dõi đơn hàng qua 14 trạng thái từ tiếp nhận đến hoàn thành
- Workflow tự động với notification system
- Quality control và approval process
- Production tracking và monitoring

### 📦 Quản lý sản phẩm
- Product catalog với dynamic properties
- Flexible pricing matrix based on combinations
- File management cho product specifications
- Advanced search và filtering capabilities

### 👥 Quản lý khách hàng
- Complete customer profiles với business information
- Credit management và debt tracking
- Transaction history và balance calculation
- Customer relationship management

### 💰 Quản lý tài chính
- Order pricing và quotation management
- Deposit và payment tracking
- Automatic balance calculations
- Financial reporting và analytics

### 📄 Quản lý file và documents
- Secure file upload và storage
- Multi-entity file associations
- Access control và permission management
- Document versioning support

## Architecture Overview

### Technology Stack
- **Backend**: .NET 9.0, ASP.NET Core MVC
- **Framework**: ASP.NET Boilerplate (ABP)
- **Database**: SQL Server với Entity Framework Core
- **Frontend**: HTML5/CSS3, JavaScript/jQuery, Bootstrap
- **UI Components**: DataTables, Select2, Dropzone.js
- **Real-time**: SignalR for notifications

### Project Structure
```
├── src/
│   ├── InTN.Core/                 # Domain entities và business logic
│   ├── InTN.Application/          # Application services và DTOs
│   ├── InTN.EntityFrameworkCore/  # Data access layer
│   ├── InTN.Web.Core/            # Web infrastructure
│   ├── InTN.Web.Mvc/             # MVC web application
│   ├── InTN.Web.Host/            # API host
│   └── InTN.Migrator/            # Database migrations
├── test/
│   ├── InTN.Tests/               # Unit tests
│   └── InTN.Web.Tests/           # Integration tests
├── ImportExcel/                   # Excel import utility
└── docs/                         # Documentation
```

## Business Workflow

### Order Processing Lifecycle
1. **Tiếp nhận yêu cầu** - Initial customer request
2. **Báo giá** - Quotation preparation
3. **Xác nhận đơn** - Order confirmation
4. **Thiết kế** - Design phase
5. **Duyệt mẫu** - Sample approval
6. **Đặt cọc** - Deposit payment
7. **In test** - Test printing
8. **Xác nhận test** - Test approval
9. **Sản xuất** - Full production
10. **Gia công** - Post-processing
11. **Kiểm tra QC** - Quality control
12. **Giao hàng** - Delivery
13. **Nghiệm thu** - Final acceptance

## Database Schema

### Core Entities
- **Product**: Product catalog với dynamic properties
- **Order**: Order management với detailed workflow tracking
- **Customer**: Customer information với financial data
- **OrderDetail**: Order line items với pricing
- **Transaction**: Financial transactions và payments
- **FileUpload**: File management với polymorphic associations

### Key Relationships
- Customer → Orders (One-to-Many)
- Order → OrderDetails (One-to-Many)
- Product → OrderDetails (One-to-Many)
- Customer → Transactions (One-to-Many)

## Setup và Installation

### Prerequisites
- .NET 9.0 SDK
- SQL Server 2019+
- Visual Studio 2022 hoặc VS Code
- Node.js (for frontend build tools)

### Database Setup
```sql
-- Create database
CREATE DATABASE InTN;

-- Update connection string in appsettings.json
"ConnectionStrings": {
  "Default": "Server=.;Database=InTN;Trusted_Connection=true;"
}
```

### Build và Run
```bash
# Restore packages
dotnet restore

# Update database
dotnet ef database update --project src/InTN.EntityFrameworkCore

# Run application
dotnet run --project src/InTN.Web.Mvc
```

### First Run Setup
1. Navigate to `https://localhost:5001`
2. Default admin login: `admin / 123qwe`
3. Configure system settings
4. Import initial data if needed

## Key Features Detail

### Product Management
- **Dynamic Properties**: Flexible product attributes stored as JSON
- **Pricing Matrix**: Complex pricing based on property combinations
- **File Attachments**: Support for multiple file types per product
- **Search & Filter**: Advanced search capabilities with real-time filtering

### Order Workflow
- **Status Tracking**: Visual workflow với color-coded status
- **Approval Process**: Multi-step approval for design và quality
- **Notification System**: Real-time updates via SignalR
- **Document Management**: File attachments at every workflow stage

### Customer Management
- **Profile Management**: Complete customer information tracking
- **Credit Control**: Automatic credit limit enforcement
- **Balance Tracking**: Real-time balance calculations
- **Transaction History**: Complete audit trail of all transactions

### Financial Features
- **Quotation Management**: Professional quotation generation
- **Payment Tracking**: Multi-payment support với deposit handling
- **VAT Calculations**: Automatic tax calculations
- **Reporting**: Financial reports và customer statements

## Configuration

### Application Settings
```json
{
  "ConnectionStrings": {
    "Default": "Server=.;Database=InTN;Trusted_Connection=true;"
  },
  "Authentication": {
    "JwtBearer": {
      "IsEnabled": "true",
      "SecurityKey": "InTN_SecurityKey",
      "Issuer": "InTN",
      "Audience": "InTN"
    }
  }
}
```

### Feature Configuration
- **File Upload**: Configure max file size và allowed types
- **Email Service**: SMTP settings for notifications
- **Security**: JWT token configuration
- **Localization**: Multi-language support settings

## Security Features

### Authentication & Authorization
- **Role-based Access**: Granular permission system
- **JWT Support**: Token-based authentication for APIs
- **Session Management**: Secure session handling
- **Password Policy**: Configurable password requirements

### Data Security
- **Audit Logging**: Complete audit trail for all operations
- **Data Encryption**: Sensitive data encryption
- **File Security**: Secure file upload và access control
- **SQL Injection Protection**: Parameterized queries và ORM protection

## Performance Optimizations

### Database Performance
- **Indexing Strategy**: Optimized indexes for common queries
- **Query Optimization**: EF Core query optimization
- **Caching**: Memory caching for frequently accessed data
- **Connection Pooling**: Efficient database connection management

### Frontend Performance
- **Asset Bundling**: CSS và JS bundling và minification
- **Lazy Loading**: On-demand loading of heavy components
- **AJAX Optimization**: Efficient AJAX calls với proper caching
- **Image Optimization**: Optimized image serving

## API Documentation

### RESTful APIs
- **Product API**: CRUD operations cho products
- **Order API**: Order management endpoints
- **Customer API**: Customer data management
- **File API**: File upload và download endpoints

### API Authentication
```javascript
// Example API call với JWT token
fetch('/api/orders', {
  headers: {
    'Authorization': 'Bearer ' + token,
    'Content-Type': 'application/json'
  }
});
```

## Testing Strategy

### Unit Testing
- **Domain Logic Testing**: Business rule validation
- **Service Testing**: Application service testing
- **Repository Testing**: Data access testing

### Integration Testing
- **API Testing**: End-to-end API testing
- **Database Testing**: Database integration testing
- **UI Testing**: Automated UI testing với Selenium

## Deployment

### Development Environment
- Local development với IIS Express
- LocalDB hoặc SQL Server Express
- Hot reload support for rapid development

### Production Deployment
- IIS hosting với proper security configuration
- SQL Server với backup và recovery strategy
- Load balancing support for high availability
- Monitoring và logging integration

## Documentation Files

Detailed documentation available in:

- **[TECHNOLOGY_OVERVIEW.md](./TECHNOLOGY_OVERVIEW.md)** - Comprehensive technology stack analysis
- **[DATABASE_SCHEMA_DOCUMENTATION.md](./DATABASE_SCHEMA_DOCUMENTATION.md)** - Complete database schema documentation
- **[BUSINESS_WORKFLOWS_DOCUMENTATION.md](./BUSINESS_WORKFLOWS_DOCUMENTATION.md)** - Detailed business process workflows

## Contributing

### Development Guidelines
1. Follow Clean Architecture principles
2. Maintain comprehensive unit test coverage
3. Use proper error handling và logging
4. Follow coding standards và conventions
5. Document all public APIs

### Code Review Process
1. Create feature branch from main
2. Implement changes với tests
3. Submit pull request với proper description
4. Code review và approval required
5. Merge after all checks pass

## Support và Maintenance

### Monitoring
- Application performance monitoring
- Error tracking và alerting
- Database performance monitoring
- User activity tracking

### Backup Strategy
- Daily database backups
- File storage backups
- Configuration backups
- Disaster recovery procedures

## License

This project is proprietary software for InTN company internal use.

## Contact

For technical support hoặc questions about the system:
- Development Team: [contact information]
- System Administrator: [contact information]
- Business Users: [internal help desk]