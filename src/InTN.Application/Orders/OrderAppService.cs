﻿using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using InTN.Orders.Dto;
using InTN.Entities;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.IO;
using Abp.Authorization;
using InTN.Authorization;
using Abp.Json;
using InTN.IdentityCodes;
using InTN.Commons;
using System.Collections.Generic;
using Newtonsoft.Json;
using Abp.Notifications;
using InTN.Authorization.Roles;
using InTN.Authorization.Users;
using Abp;
using InTN.Processes;
using InTN.Users;
using Microsoft.AspNetCore.Http;


namespace InTN.Orders
{
    public class OrderAppService : AsyncCrudAppService<Order, OrderDto, int, PagedResultRequestDto, OrderDto, OrderDto>, IOrderAppService
    {
        private readonly IRepository<OrderLog> _orderLogRepository;
        private readonly IRepository<OrderAttachment> _orderAttachmentRepository;
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerBalanceHistory> _customerBalanceHistoryRepository;
        private readonly IRepository<OrderDetail> _orderDetailRepository;
        private readonly IRepository<Process> _processRepository;
        private readonly IRepository<ProcessStep> _processStepRepository;
        private readonly IRepository<FileUpload> _fileUploadRepository;

        private readonly IIdentityCodeAppService _identityCodeAppService;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IUserAppService _userAppService;


        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        private readonly IntnAppSession _intnAppSession;


        public OrderAppService(IRepository<Order> repository,
            IRepository<OrderLog> orderLogRepository,
            IRepository<Transaction> transactionRepository,
            IRepository<OrderAttachment> orderAttachmentRepository,
            IRepository<Customer> customerRepository,
            IRepository<CustomerBalanceHistory> customerBalanceHistoryRepository,
            IRepository<OrderDetail> orderDetailRepository,
            IRepository<Process> processRepository,
            IRepository<ProcessStep> processStepRepository,
            IRepository<FileUpload> fileUploadRepository,

            IIdentityCodeAppService identityCodeRepository,
            INotificationPublisher notificationPublisher,
            IUserAppService userAppService,
            RoleManager roleManager,
            UserManager userManager,
            IntnAppSession intnAppSession

            )
            : base(repository)
        {
            _orderAttachmentRepository = orderAttachmentRepository;
            _orderLogRepository = orderLogRepository;
            _transactionRepository = transactionRepository;
            _customerRepository = customerRepository;
            _customerBalanceHistoryRepository = customerBalanceHistoryRepository;
            _identityCodeAppService = identityCodeRepository;
            _orderDetailRepository = orderDetailRepository;
            _notificationPublisher = notificationPublisher;
            _processRepository = processRepository;
            _processStepRepository = processStepRepository;
            _fileUploadRepository = fileUploadRepository;

            _userAppService = userAppService;
            _roleManager = roleManager;

            _userManager = userManager;
            _roleManager = roleManager;
            _intnAppSession = intnAppSession;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
       // [AbpAuthorize(PermissionNames.Fn_Orders_CreateQuotation)]
        [HttpPost]
        public async Task<Order> CreateNewAsync(CreateOrderDto input)
        {
            // Validate input
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), "Input cannot be null");
            }
            if (string.IsNullOrEmpty(input.OrderCode))
            {
                throw new ArgumentException("Order code cannot be null or empty", nameof(input.OrderCode));
            }

            if (string.IsNullOrEmpty(input.CustomerName))
            {
                throw new ArgumentException("Customer name cannot be null or empty", nameof(input.CustomerName));
            }
            if (string.IsNullOrEmpty(input.CustomerPhone))
            {
                throw new ArgumentException("Customer phone cannot be null or empty", nameof(input.CustomerPhone));
            }

            if (input.NewCustomer)
            {
                // Tạo khách hàng mới
                var newCustomer = new Customer
                {
                    Name = input.CustomerName,
                    PhoneNumber = input.CustomerPhone,
                    Email = input.CustomerEmail,
                    Address = input.CustomerAddress,
                    TotalDebt = input.CustomerTotalDebt,
                    CreditLimit = input.CreditLimit,
                    //   TotalOrderAmount = input.CustomerTotalOrderAmount,
                };

                // Lưu khách hàng mới vào cơ sở dữ liệu
                var customerId = await _customerRepository.InsertAndGetIdAsync(newCustomer);

                // Gán ID khách hàng mới vào đơn hàng
                input.CustomerId = customerId;
                input.CustomerName = newCustomer.Name;
            }

            input.OrderDate = DateTime.Now; // Set the current date and time as the order date
        //    input.Status = (int)OrderStatus.ReceivedRequest; // Set default status to 0 (Pending)


            try
            {
                // var order = ObjectMapper.Map<Order>(input);

                var order = new Order
                {
                    OrderCode = input.OrderCode,
                    CustomerId = input.CustomerId,
                    OrderDate = input.OrderDate,
                    Status = input.Status,
                    Note = input.Note,
                    FileUrl = input.FileUrl,
                    CustomerName = input.CustomerName,
                    CustomerAddress = input.CustomerAddress,
                    CustomerPhone = input.CustomerPhone,
                    CustomerEmail = input.CustomerEmail,
                    PaymentStatus = input.PaymentStatus,
                    CustomerGender = input.CustomerGender,
                    CustomerType = input.CustomerType,
                    DeliveryMethod = input.DeliveryMethod,
                    ExpectedDeliveryDate = input.ExpectedDeliveryDate,
                    IsRequireTestSample = input.IsRequireTestSample,
                    IsExportInvoice = input.IsExportInvoice,
                    IsStoreSample = input.IsStoreSample,
                    IsReceiveByOthers = input.IsReceiveByOthers,
                    OtherRequirements = input.OtherRequirements,
                    // FileIds = input.FileIds,
                    TotalProductAmount = input.TotalProductAmount,
                    TotalDeposit = input.TotalDeposit,
                    DeliveryFee = input.DeliveryFee,
                    VatRate = input.VatRate,
                    VatAmount = input.VatAmount,
                    DiscountAmount = input.DiscountAmount,
                    TotalAmount = input.TotalAmount,
                    TotalCustomerPay = input.TotalCustomerPay,
                    StepId = input.StepId,
                    ProcessId = input.ProcessId,
                    ShippingMethod = input.ShippingMethod,
                    

                };

                var orderId = await Repository.InsertAndGetIdAsync(order);

                decimal totalProductPrice = 0;
                // Duyệt danh sách orderDetails để tạo mới từng OrderDetail
                if (input.OrderDetails != null && input.OrderDetails.Count > 0)
                {
                    foreach (var detailDto in input.OrderDetails)
                    {
                        // Tìm kiếm sản phẩm trong kho dựa trên ProductId
                        var orderDetail = new OrderDetail
                        {
                            OrderId = orderId,
                            ProductId = detailDto.ProductId,
                            ProductName = detailDto.ProductName,
                            UnitPrice = detailDto.UnitPrice,
                            Quantity = detailDto.Quantity,
                            TotalProductPrice = detailDto.UnitPrice * detailDto.Quantity,
                            FileId = detailDto.FileId,
                            FileUrl = detailDto.FileUrl,
                            Note = detailDto.Note,
                            Properties = JsonConvert.SerializeObject(detailDto.Properties),
                            NoteIds = string.Join(",", detailDto.NoteIds),
                            FileType = detailDto.FileType
                        };
                        totalProductPrice += orderDetail.TotalProductPrice;
                        // Lưu OrderDetail vào cơ sở dữ liệu
                        await _orderDetailRepository.InsertAsync(orderDetail);
                    }
                }

                // Cập nhật tổng tiền sản phẩm nếu chưa có
                if (!order.TotalProductAmount.HasValue || order.TotalProductAmount == 0)
                {
                    order.TotalProductAmount = totalProductPrice;
                    await Repository.UpdateAsync(order);
                }

                await _orderLogRepository.InsertAsync(new OrderLog
                {
                    OrderId = order.Id,
                    Action = "Create",
                    NewValue = order.ToJsonString(),
                });

                // Lấy quy trình liên quan đến đơn hàng
                // Lấy ra bước tiếp theo trong quy trình
                // Lấy ra danh sách người nhận của bước tiếp theo (trong bước có roleIds -> lấy user theo roleIds)
                var processStep = _processStepRepository.Get(order.StepId.Value);

                if (processStep != null)
                {
                    var nextStepIds = processStep.NextStepIds.Split(",");
                    var nextSteps = await _processStepRepository.GetAllListAsync(x => nextStepIds.Contains(x.Id.ToString()));
                    bool noticeToAll = false;
                    var roleIds = new List<int>();
                    if (nextSteps.Any())
                    {
                        // lấy ra danh sách cách roleIds theo nextStep

                        foreach (var step in nextSteps)
                        {
                            if (!string.IsNullOrEmpty(step.RoleIds))
                            {
                                if (step.RoleIds == "*")
                                {
                                    noticeToAll = true; // Thông báo cho tất cả người dùng
                                    break; // Không cần kiểm tra thêm
                                }
                                else
                                {
                                    var stepRoleIds = step.RoleIds.Split(',').Select(int.Parse).ToList();
                                    roleIds.AddRange(stepRoleIds);
                                }
                            }
                        }
                    }

                    List<UserIdentifier> usersReceiptNotify = new List<UserIdentifier>();

                    if (noticeToAll)
                    {
                        // Lấy toàn bộ danh sách user
                        usersReceiptNotify = await _userAppService.GetAllListUserIdentifierAsync();
                    }
                    else
                    {
                        usersReceiptNotify = await _userAppService.GetAllListUserIdentifierByRoleIdsAsync(roleIds);
                    }


                    // Gửi notification
                    var data = new NotificationData();
                    data["OrderId"] = order.Id;
                    data["OrderCode"] = order.OrderCode;
                    data["CreatorName"] = _intnAppSession.UserName; // hoặc từ _userManager
                    data["Message"] = $"Đơn hàng mới {order.OrderCode} đã được tạo bởi {_intnAppSession.UserName}";

                    await _notificationPublisher.PublishAsync(
                        "Order.Created",
                        data,
                        userIds: usersReceiptNotify.ToArray()
                    );
                }

                return order;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPut]
        public async Task CreateQuotationAsync([FromForm] OrderQuotationUploadDto input)
        {
            if (input.Attachments != null && input.Attachments.Any() && input.TotalAmount > 0)
            {
                var order = Repository.Get(input.OrderId);
                if (order != null)
                {
                    var orderLog = new OrderLog()
                    {
                        OrderId = order.Id,
                        Action = "CreateQuotation",
                        OldValue = order.ToJsonString(),
                    };

                    order.TotalAmount = input.TotalAmount;
                    order.Status = (int)OrderStatus.Quoted; // Set default status to 0 (Pending)
                    Repository.Update(order);

                    orderLog.NewValue = order.ToJsonString();

                    foreach (var file in input.Attachments)
                    {

                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream); // Đọc dữ liệu từ file

                            var attachment = new OrderAttachment
                            {
                                OrderId = input.OrderId,
                                FileName = file.FileName,
                                FileType = file.ContentType,    // Loại file (image/jpeg, image/png)
                               // FileContent = memoryStream.ToArray(), // Dữ liệu nhị phân của hình ảnh
                                FileSize = file.Length,
                                Type = (int)OrderAttachmentType.Invoice,    // Loại file (image/jpeg, image/png)
                            };
                            try
                            {
                                await _orderAttachmentRepository.InsertAsync(attachment);
                            }
                            catch (System.Exception ex)
                            {

                            }
                            // Lưu hình ảnh vào database
                        }
                    }
                    await _orderLogRepository.InsertAsync(orderLog);
                }

            }
        }

        /// <summary>
        /// Thực hiện duyệt mẫu thiết kế
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task ApproveDesignAsync([FromForm] OrderDesignUploadDto input)
        {
            if (input.Attachments != null && input.Attachments.Any())
            {
                var order = Repository.Get(input.OrderId);
                if (order != null)
                {
                    var orderLog = new OrderLog()
                    {
                        OrderId = order.Id,
                        Action = "ApproveDesign",
                        OldValue = order.ToJsonString(),
                    };
                    order.Status = (int)OrderStatus.DesignApproved; // Set default status to 0 (Pending)
                    Repository.Update(order);
                    orderLog.NewValue = order.ToJsonString();

                    foreach (var file in input.Attachments)
                    {

                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream); // Đọc dữ liệu từ file

                            var attachment = new OrderAttachment
                            {
                                OrderId = input.OrderId,
                                FileName = file.FileName,
                                FileType = file.ContentType,    // Loại file (image/jpeg, image/png)
                                //FileContent = memoryStream.ToArray(), // Dữ liệu nhị phân của hình ảnh
                                FileSize = file.Length,
                                Type = (int)OrderAttachmentType.DesignSample,    // Loại file (image/jpeg, image/png)
                            };
                            try
                            {
                                await _orderAttachmentRepository.InsertAsync(attachment);
                            }
                            catch (System.Exception ex)
                            {

                            }
                            // Lưu hình ảnh vào database
                        }
                    }
                    await _orderLogRepository.InsertAsync(orderLog);
                }


            }
        }

        /// <summary>
        /// Thực hiện cập nhật trạng thái đơn hàng thành đã đặt cọc
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [HttpPut]
        public async Task UpdateStatusToDepositedAsync([FromForm] OrderDepositUploadDto input)
        {

            if (input.Attachments != null && input.Attachments.Any())
            {
                var order = Repository.Get(input.OrderId);
                if (order == null)
                {
                    throw new ArgumentException($"Order with ID {input.OrderId} not found", nameof(input.OrderId));
                }

                var orderLog = new OrderLog()
                {
                    OrderId = order.Id,
                    Action = "UpdateStatusToDeposited",
                    OldValue = order.ToJsonString(),
                };
                order.TotalDeposit = input.DepositAmount;
                order.Status = (int)OrderStatus.Deposited; // Set default status to 0 (Pending)
                Repository.Update(order);

                foreach (var file in input.Attachments)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream); // Đọc dữ liệu từ file

                        var attachment = new OrderAttachment
                        {
                            OrderId = input.OrderId,
                            FileName = file.FileName,
                            FileType = file.ContentType,    // Loại file (image/jpeg, image/png)
                          //  FileContent = memoryStream.ToArray(), // Dữ liệu nhị phân của hình ảnh
                            FileSize = file.Length,
                            Type = (int)OrderAttachmentType.DesignSample,    // Loại file (image/jpeg, image/png)
                        };
                        try
                        {
                            await _orderAttachmentRepository.InsertAsync(attachment);
                        }
                        catch (System.Exception ex)
                        {

                        }
                        // Lưu hình ảnh vào database
                    }
                }

                orderLog.NewValue = order.ToJsonString();
                await _orderLogRepository.InsertAsync(orderLog);

                // Ghi lại giao dịch
                var transaction = new Transaction
                {
                    CustomerId = order.CustomerId,
                    OrderId = order.Id,
                    Amount = input.DepositAmount,
                    Description = "Đặt cọc đơn hàng",
                };

                await _transactionRepository.InsertAsync(transaction);

            }
        }

        /// <summary>
        /// Thực hiện in test
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// 
        [HttpPut]
        public async Task UpdateStatusToPrintedTestAsync(int id)
        {
            // Retrieve the order by ID
            var order = await Repository.GetAsync(id);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {id} not found", nameof(id));
            }
            var orderLog = new OrderLog()
            {
                OrderId = order.Id,
                Action = "UpdateStatusToPrintedTest",
                OldValue = order.ToJsonString(),
            };
            // Update the status to "Printed Test"
            order.Status = (int)OrderStatus.PrintingTest;

            // Save the changes
            await Repository.UpdateAsync(order);

            orderLog.NewValue = order.ToJsonString();
            await _orderLogRepository.InsertAsync(orderLog);
        }

        [HttpPut]
        public async Task ConfirmPrintedTestAsync(int id)
        {
            // Retrieve the order by ID
            var order = await Repository.GetAsync(id);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {id} not found", nameof(id));
            }

            var orderLog = new OrderLog()
            {
                OrderId = order.Id,
                Action = "ConfirmPrintedTest",
                OldValue = order.ToJsonString(),
            };
            // Update the status to "Test Printed Confirmed"
            order.Status = (int)OrderStatus.PrintingTestConfirmed;

            // Save the changes
            await Repository.UpdateAsync(order);

            orderLog.NewValue = order.ToJsonString();
            await _orderLogRepository.InsertAsync(orderLog);
        }

        [HttpPut]
        public async Task PerformPrintingAsync(int id)
        {
            // Retrieve the order by ID
            var order = await Repository.GetAsync(id);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {id} not found", nameof(id));
            }
            var orderLog = new OrderLog()
            {
                OrderId = order.Id,
                Action = "PerformPrinting",
                OldValue = order.ToJsonString(),
            };
            // Update the status to "Printing"
            order.Status = (int)OrderStatus.Printing;

            // Save the changes
            await Repository.UpdateAsync(order);

            orderLog.NewValue = order.ToJsonString();
            await _orderLogRepository.InsertAsync(orderLog);
        }

        [HttpPut]
        public async Task PerformProcessingAsync(int id)
        {
            // Retrieve the order by ID
            var order = await Repository.GetAsync(id);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {id} not found", nameof(id));
            }

            var orderLog = new OrderLog()
            {
                OrderId = order.Id,
                Action = "PerformProcessing",
                OldValue = order.ToJsonString(),
            };
            // Update the status to "Processing"
            order.Status = (int)OrderStatus.Processing;

            // Save the changes
            await Repository.UpdateAsync(order);

            orderLog.NewValue = order.ToJsonString();
            await _orderLogRepository.InsertAsync(orderLog);
        }

        [HttpPut]
        public async Task ShipOrderAsync(int id)
        {
            // Retrieve the order by ID
            var order = await Repository.GetAsync(id);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {id} not found", nameof(id));
            }

            var orderLog = new OrderLog()
            {
                OrderId = order.Id,
                Action = "ShipOrder",
                OldValue = order.ToJsonString(),
            };
            // Update the status to "Shipped"
            order.Status = (int)OrderStatus.Delivering;

            // Save the changes
            await Repository.UpdateAsync(order);

            orderLog.NewValue = order.ToJsonString();
            await _orderLogRepository.InsertAsync(orderLog);
        }

        [HttpPut]
        public async Task CompleteOrderAsync(int id)
        {
            // Retrieve the order by ID
            var order = await Repository.GetAsync(id);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {id} not found", nameof(id));
            }

            var orderLog = new OrderLog()
            {
                OrderId = order.Id,
                Action = "CompleteOrder",
                OldValue = order.ToJsonString(),
            };

            // Update the status to "Completed"
            order.Status = (int)OrderStatus.Completed;

            // Save the changes
            await Repository.UpdateAsync(order);

            orderLog.NewValue = order.ToJsonString();
            await _orderLogRepository.InsertAsync(orderLog);
        }


        [HttpPut]
        public async Task OrderDebtAsync(int id)
        {
            // Retrieve the order by ID
            var order = await Repository.GetAsync(id);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {id} not found", nameof(id));
            }

            var orderLog = new OrderLog()
            {
                OrderId = order.Id,
                Action = "Debt",
                OldValue = order.ToJsonString(),
            };

            // Update the status to "Completed"
            order.PaymentStatus = (int)OrderPaymentStatus.Debt;

            // Save the changes
            await Repository.UpdateAsync(order);

            orderLog.NewValue = order.ToJsonString();
            await _orderLogRepository.InsertAsync(orderLog);

            if (order.CustomerId.HasValue)
            {
                var customer = await _customerRepository.GetAsync(order.CustomerId.Value);
                if (customer != null)
                {

                    var identityCode = await _identityCodeAppService.GenerateNewSequentialNumberAsync("GD");

                    var transaction = new Transaction
                    {
                        TransactionCode = identityCode.Code,
                        CustomerId = order.CustomerId,
                        CustomerName = order.CustomerName,
                        OrderId = order.Id,
                        Amount = (order.TotalAmount ?? 0) - (order.TotalDeposit ?? 0),
                        Description = "Công nợ đơn hàng",
                    };
                    var transactionId = await _transactionRepository.InsertAndGetIdAsync(transaction);
                    // Ghi lại lịch sử công nợ
                    var customerBalanceHistory = new CustomerBalanceHistory
                    {
                        CustomerId = order.CustomerId.Value,
                        TransactionId = transactionId,
                        Type = 1, // Tăng công nợ
                        Amount = transaction.Amount,
                        BalanceAfterTransaction = customer.TotalDebt + transaction.Amount, // Cập nhật số dư sau giao dịch
                    };
                    await _customerBalanceHistoryRepository.InsertAsync(customerBalanceHistory);

                    customer.TotalDebt += transaction.Amount;
                    await _customerRepository.UpdateAsync(customer);
                }
            }

        }

        public async Task UpdateOrderStatusAsync(int id, int nextStepId,  int status)
        {
            // Retrieve the order by ID
            var order = await Repository.GetAsync(id);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {id} not found", nameof(id));
            }
            var orderLog = new OrderLog()
            {
                OrderId = order.Id,
                Action = "UpdateStatus",
                OldValue = order.ToJsonString()

            };
            // Update the status to the specified status
            order.Status = status;
            order.StepId = nextStepId;
            // Save the changes
            await Repository.UpdateAsync(order);
            orderLog.NewValue = order.ToJsonString();
            await _orderLogRepository.InsertAsync(orderLog);
        }

        /// <summary>
        /// Thực hiện thanh toán đơn hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task OrderPaymentAsync([FromForm] OrderTransactionUploadDto input)
        {
            byte[] fileContent = null;
            if (input.Attachments != null && input.Attachments.Any())
            {
                var order = Repository.Get(input.OrderId);
                if (order != null)
                {
                    var orderLog = new OrderLog()
                    {
                        OrderId = order.Id,
                        Action = "Payment",
                        OldValue = order.ToJsonString(),
                    };
                    order.PaymentStatus = (int)OrderPaymentStatus.Paid; // Set default status to 0 (Pending)
                    Repository.Update(order);
                    orderLog.NewValue = order.ToJsonString();

                    foreach (var file in input.Attachments)
                    {

                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream); // Đọc dữ liệu từ file
                            fileContent = memoryStream.ToArray(); // Chuyển đổi dữ liệu thành mảng byte
                            var attachment = new OrderAttachment
                            {
                                OrderId = input.OrderId,
                                FileName = file.FileName,
                                FileType = file.ContentType,    // Loại file (image/jpeg, image/png)
                                //FileContent = fileContent, // Dữ liệu nhị phân của hình ảnh
                                FileSize = file.Length,
                                Type = (int)OrderAttachmentType.DesignSample,    // Loại file (image/jpeg, image/png)
                            };
                            try
                            {
                                await _orderAttachmentRepository.InsertAsync(attachment);
                            }
                            catch (System.Exception ex)
                            {

                            }
                            // Lưu hình ảnh vào database
                        }
                    }
                    await _orderLogRepository.InsertAsync(orderLog);

                    var identity = await _identityCodeAppService.GenerateNewSequentialNumberAsync("GD");

                    // Ghi lại giao dịch
                    var transaction = new Transaction
                    {
                        TransactionCode = identity.Code,
                        CustomerId = order.CustomerId,
                        CustomerName = order.CustomerName,
                        OrderId = order.Id,
                        Amount = input.Amount,
                        Description = "Thanh toán đơn hàng",
                        FileContent = fileContent,
                        TransactionType = (int)TransactionType.OrderPayment,

                    };

                    var transactionId = await _transactionRepository.InsertAndGetIdAsync(transaction);
                }


            }
        }


        public async Task<List<OptionItemDto>> GetOrderListForSelect(string q)
        {
            try
            {
                var query = await Repository.GetAllAsync();
                query = query.Where(u =>
                u.Status == (int)OrderStatus.Completed &&
                u.PaymentStatus != (int)OrderPaymentStatus.Debt &&
                u.PaymentStatus != (int)OrderPaymentStatus.Paid
                )

                ;
                if (!string.IsNullOrEmpty(q))
                {
                    q = q.ToUpper();
                    query = query.Where(u => u.OrderCode.ToUpper().Contains(q));
                }

                return query.Select(u => new OptionItemDto
                {
                    id = u.Id.ToString(),
                    text = u.OrderCode
                }).ToList();


            }
            catch (Exception ex)
            {

            }
            return new List<OptionItemDto>();
        }


        public async Task<PagedResultDto<OrderDto>> GetAllOrderTodayAsync(PagedResultRequestDto input)
        {
            var query = base.CreateFilteredQuery(input);

            query = query.Where(x => x.CreationTime.Date == DateTime.Now.Date);

            var count = query.Count();
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var data = ObjectMapper.Map<List<OrderDto>>(query.ToList());
            return new PagedResultDto<OrderDto>()
            {
                Items = data,
                TotalCount = count
            };
        }



    }
}
