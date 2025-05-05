using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;

using InTN.Orders.Dto;
using InTN.Entities;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.IO;
using System.Net.Mail;
using Abp.Authorization;
using InTN.Authorization;
using NuGet.Protocol;
using Abp.Json;
using InTN.IdentityCodes;
using InTN.Commons;
using System.Collections.Generic;

namespace InTN.Orders
{
    public class OrderAppService : AsyncCrudAppService<Order, OrderDto, int, PagedResultRequestDto, OrderDto, OrderDto>, IOrderAppService
    {
        private readonly IRepository<OrderLog> _orderLogRepository;
        private readonly IRepository<OrderAttachment> _orderAttachmentRepository;
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerBalanceHistory> _customerBalanceHistoryRepository;

        private readonly IIdentityCodeAppService _identityCodeAppService;

        public OrderAppService(IRepository<Order> repository,
            IRepository<OrderLog> orderLogRepository,
            IRepository<Transaction> transactionRepository,
            IRepository<OrderAttachment> orderAttachmentRepository,
            IRepository<Customer> customerRepository,
            IRepository<CustomerBalanceHistory> customerBalanceHistoryRepository,
            IIdentityCodeAppService identityCodeRepository
            )
            : base(repository)
        {
            _orderAttachmentRepository = orderAttachmentRepository;
            _orderLogRepository = orderLogRepository;
            _transactionRepository = transactionRepository;
            _customerRepository = customerRepository;
            _customerBalanceHistoryRepository = customerBalanceHistoryRepository;
            _identityCodeAppService = identityCodeRepository;
        }


        /// <summary>
        /// /
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        [AbpAuthorize(PermissionNames.Fn_Orders_CreateQuotation)]
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

            input.OrderDate = DateTime.Now; // Set the current date and time as the order date
            input.Status = (int)OrderStatus.ReceivedRequest; // Set default status to 0 (Pending)

            var order = ObjectMapper.Map<Order>(input);

            order = await Repository.InsertAsync(order);
            await _orderLogRepository.InsertAsync(new OrderLog
            {
                OrderId = order.Id,
                Action = "Create",
                NewValue = order.ToJsonString(),
            });

            return order;
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
                                FileContent = memoryStream.ToArray(), // Dữ liệu nhị phân của hình ảnh
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
                                FileContent = memoryStream.ToArray(), // Dữ liệu nhị phân của hình ảnh
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
                            FileContent = memoryStream.ToArray(), // Dữ liệu nhị phân của hình ảnh
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
                Action = "CompleteOrder",
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


        


        public async Task<List<OptionItemDto>> GetOrderListForSelect(string q)
        {
            try
            {
                var query = await Repository.GetAllAsync();
                query = query.Where(u => 
                u.Status == (int) OrderStatus.Completed && 
                u.PaymentStatus != (int)OrderPaymentStatus.Debt  &&  
                u.PaymentStatus != (int)OrderPaymentStatus.Paid
                )
                
                ;
                if (!string.IsNullOrEmpty(q))
                {
                    q = q.ToUpper();
                    query = query.Where(u => u.OrderCode.ToUpper().Contains(q)  );
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
    }
}
