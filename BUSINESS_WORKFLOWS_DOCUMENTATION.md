# InTN - Business Workflows và Process Documentation

## Tổng quan Business Domain

InTN là hệ thống quản lý quy trình sản xuất in ấn và gia công, được thiết kế để theo dõi toàn bộ lifecycle của đơn hàng từ lúc tiếp nhận yêu cầu đến khi hoàn thành nghiệm thu.

## Core Business Workflows

### 1. Order Management Workflow

#### 1.1 Order Lifecycle States
Hệ thống theo dõi đơn hàng qua 14 trạng thái chính:

```
1. Tiếp nhận yêu cầu → 2. Đã báo giá → 3. Đã xác nhận đơn → 4. Đang thiết kế 
→ 5. Đang chờ duyệt mẫu → 6. Đã duyệt mẫu → 7. Đã đặt cọc → 8. Đang in test 
→ 9. Xác nhận in test (Ok) → 10. Đang in → 11. Đang gia công → 12. Đã kiểm tra QC 
→ 13. Đang giao hàng → 14. Hoàn thành nghiệm thu
```

#### 1.2 Detailed Workflow Steps

**Phase 1: Sales & Quotation (Giai đoạn bán hàng)**
- **Step 1**: Tiếp nhận yêu cầu khách hàng
  - Input: Customer requirements, specifications
  - Actions: Create order record, assign customer information
  - Output: Order với status = 1

- **Step 2**: Báo giá
  - Input: Product requirements, quantities
  - Actions: Calculate pricing, create quotation
  - Output: Order với status = 2, quotation document

- **Step 3**: Xác nhận đơn hàng
  - Input: Customer approval của quotation
  - Actions: Confirm order details, lock pricing
  - Output: Order với status = 3

**Phase 2: Design & Approval (Giai đoạn thiết kế)**
- **Step 4**: Thiết kế
  - Input: Design requirements, customer specs
  - Actions: Create/modify designs, prepare samples
  - Output: Order với status = 4, design files

- **Step 5**: Chờ duyệt mẫu
  - Input: Completed design/sample
  - Actions: Submit for customer approval
  - Output: Order với status = 5

- **Step 6**: Duyệt mẫu
  - Input: Customer approval của design
  - Actions: Finalize design, approve for production
  - Output: Order với status = 6

**Phase 3: Financial & Production Preparation**
- **Step 7**: Đặt cọc
  - Input: Customer deposit payment
  - Actions: Record transaction, update payment status
  - Output: Order với status = 7, transaction record

**Phase 4: Production Process (Giai đoạn sản xuất)**
- **Step 8**: In test
  - Input: Approved design, production setup
  - Actions: Print test samples for quality check
  - Output: Order với status = 8, test samples

- **Step 9**: Xác nhận in test OK
  - Input: Quality check của test samples
  - Actions: Approve test quality, proceed to full production
  - Output: Order với status = 9

- **Step 10**: Đang in
  - Input: Approved test samples
  - Actions: Full production printing
  - Output: Order với status = 10, printed materials

- **Step 11**: Gia công
  - Input: Printed materials
  - Actions: Post-processing (cutting, binding, finishing)
  - Output: Order với status = 11, finished products

**Phase 5: Quality & Delivery (Giai đoạn kiểm tra và giao hàng)**
- **Step 12**: Kiểm tra QC
  - Input: Finished products
  - Actions: Quality control inspection
  - Output: Order với status = 12, QC approval

- **Step 13**: Giao hàng
  - Input: QC approved products
  - Actions: Package and ship to customer
  - Output: Order với status = 13, delivery tracking

- **Step 14**: Hoàn thành nghiệm thu
  - Input: Customer acceptance
  - Actions: Final payment processing, close order
  - Output: Order với status = 14, completed transaction

### 2. Product Management Workflow

#### 2.1 Product Creation Process
1. **Product Definition**
   - Input: Product specifications, category, supplier info
   - Actions: Create product record với basic information
   - Output: Product entity với unique code

2. **Property Configuration**
   - Input: Product attributes (size, color, material, etc.)
   - Actions: Configure dynamic properties as JSON
   - Output: Flexible product attributes

3. **Pricing Setup**
   - Input: Cost information, margin requirements
   - Actions: Create pricing combinations based on properties
   - Output: ProductPriceCombination records

4. **File Attachment**
   - Input: Product images, specifications, samples
   - Actions: Upload and associate files
   - Output: FileUpload associations

#### 2.2 Product Search & Selection
1. **Advanced Search**
   - Filters: Category, Type, Supplier, Brand, Properties
   - Full-text search on name and description
   - Real-time filtering with AJAX

2. **Property-based Pricing**
   - Dynamic price calculation based on selected properties
   - Quantity-based pricing tiers
   - Real-time price updates

### 3. Customer Management Workflow

#### 3.1 Customer Onboarding
1. **Customer Registration**
   - Input: Basic information (name, contact, address)
   - Actions: Generate customer code, create record
   - Output: Customer entity với unique code

2. **Business Information Setup**
   - Input: Company details, tax information
   - Actions: Configure business profile
   - Output: Complete customer profile

3. **Credit Management Setup**
   - Input: Credit limit requirements
   - Actions: Set credit limits, initialize balance
   - Output: Financial profile established

#### 3.2 Customer Relationship Management
1. **Order History Tracking**
   - Track all orders associated with customer
   - Calculate lifetime value
   - Identify purchasing patterns

2. **Balance Management**
   - Real-time debt tracking
   - Automatic balance updates from transactions
   - Credit limit enforcement

3. **Communication History**
   - Log all customer interactions
   - Track order notes and special requirements
   - Maintain relationship history

### 4. Financial Management Workflow

#### 4.1 Transaction Processing
1. **Deposit Handling**
   - Input: Customer deposit payment
   - Actions: Create transaction record, update order
   - Output: Updated order payment status

2. **Final Payment Processing**
   - Input: Final payment from customer
   - Actions: Complete payment, close financial obligations
   - Output: Fully paid order status

3. **Debt Management**
   - Track outstanding balances
   - Generate payment reminders
   - Manage credit limits

#### 4.2 Financial Reporting
1. **Customer Balance Reports**
   - Real-time balance calculations
   - Historical balance changes
   - Credit utilization analysis

2. **Order Financial Summary**
   - Total order values
   - Payment status tracking
   - Revenue recognition

### 5. File Management Workflow

#### 5.1 File Upload Process
1. **File Upload**
   - Input: Files from users (images, docs, specs)
   - Actions: Validate, store, create metadata
   - Output: FileUpload entities với paths

2. **Association Management**
   - Link files to entities (Products, Orders)
   - Support multiple file types per entity
   - Maintain file organization

3. **File Access Control**
   - Permission-based file access
   - Secure file serving
   - Download tracking

### 6. Notification & Communication Workflow

#### 6.1 Status Change Notifications
1. **Order Status Updates**
   - Automatic notifications on status changes
   - Email notifications to stakeholders
   - Real-time UI updates via SignalR

2. **Payment Reminders**
   - Automated payment due notifications
   - Credit limit warnings
   - Overdue payment alerts

#### 6.2 Internal Communication
1. **Order Notes & Comments**
   - Internal communication on orders
   - Status change reasons
   - Special handling instructions

2. **Process Documentation**
   - Step-by-step process tracking
   - Quality check documentation
   - Issue tracking và resolution

## Business Rules và Constraints

### 1. Order Management Rules
- Order codes must be unique và auto-generated
- Status transitions must follow defined workflow
- Cannot skip required steps in workflow
- Payment must be confirmed before production starts
- Quality checks required before delivery

### 2. Financial Rules
- Deposits must be recorded before production
- Credit limits cannot be exceeded
- All transactions must have proper documentation
- Balance calculations must be accurate và real-time

### 3. Product Rules
- Product codes must be unique
- Pricing must be configured for all property combinations
- Active products only available for new orders
- File attachments must be properly categorized

### 4. Customer Rules
- Customer codes auto-generated và unique
- Contact information must be validated
- Credit limits enforced for all transactions
- Business information required for invoice generation

## Performance & Scalability Considerations

### 1. Order Volume Handling
- Efficient status update processing
- Batch operations for bulk updates
- Optimized queries for order searches
- Archiving strategy for completed orders

### 2. Product Catalog Scalability
- Efficient property-based searching
- Cached pricing calculations
- Optimized image serving
- Search index optimization

### 3. Customer Data Management
- Fast customer lookup và search
- Efficient balance calculations
- Optimized transaction processing
- Historical data archiving

## Integration Points

### 1. External System Integration
- Accounting system integration for invoicing
- Payment gateway integration
- Shipping provider APIs
- Email service integration

### 2. Internal System Integration
- File storage systems
- Backup và disaster recovery
- Monitoring và alerting
- Analytics và reporting

## Security Considerations

### 1. Data Security
- Customer data encryption
- File access controls
- Audit trail maintenance
- Secure authentication

### 2. Process Security
- Role-based workflow access
- Status change authorization
- Financial transaction security
- File upload validation

## Future Enhancement Opportunities

### 1. Automation Improvements
- Automated status transitions
- Smart notification routing
- Predictive analytics for delays
- Automated quality checks

### 2. Customer Experience
- Customer portal development
- Mobile app for tracking
- Self-service capabilities
- Real-time status updates

### 3. Analytics & Reporting
- Advanced business intelligence
- Predictive analytics
- Performance dashboards
- Custom reporting tools

## Metrics & KPIs

### 1. Operational Metrics
- Order processing time by stage
- Quality rejection rates
- On-time delivery performance
- Customer satisfaction scores

### 2. Financial Metrics
- Revenue per order
- Profit margins by product type
- Customer lifetime value
- Days sales outstanding

### 3. Process Efficiency
- Time spent in each workflow stage
- Bottleneck identification
- Resource utilization
- Productivity measurements