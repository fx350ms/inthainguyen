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

namespace InTN.Orders
{
    public class OrderAppService : AsyncCrudAppService<Order, OrderDto, int, PagedResultRequestDto, OrderDto, OrderDto>, IOrderAppService
    {

        public readonly IRepository<OrderAttachment> _orderAttachmentRepository;
        public OrderAppService(IRepository<Order> repository,
            IRepository<OrderAttachment> orderAttachmentRepository)
            : base(repository)
        {
            _orderAttachmentRepository = orderAttachmentRepository;
        }

        public override async Task<PagedResultDto<OrderDto>> GetAllAsync(PagedResultRequestDto input)
        {
            try
            {
                var data = await base.GetAllAsync(input);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<OrderDto> CreateOrderWithAttachmentAsync(OrderDto input)
        {
            return await base.CreateAsync(input);
        }

        
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
            return await Repository.InsertAsync(order);
            //  return await base.CreateAsync(input);
        }
        [HttpPut]
        public async Task CreateQuotation([FromForm] OrderQuotationUploadDto input)
        {
            if (input.Attachments != null && input.Attachments.Any() && input.TotalAmount > 0)
            {
                var order = Repository.Get(input.OrderId);
                if (order != null)
                {
                    order.TotalAmount = input.TotalAmount;
                    order.Status = (int)OrderStatus.Quoted; // Set default status to 0 (Pending)
                    Repository.Update(order);
                }

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
            }
        }

        //public async Task ApproveDesign(int id)
        //{
        //    var order = await Repository.GetAsync(id);
        //    if (order != null)
        //    {
        //        order.Status = (int)OrderStatus.DesignApproved; //  
        //        await Repository.UpdateAsync(order);
        //    }
        //}

        [HttpPut]
        public async Task ApproveDesign([FromForm] OrderDesignUploadDto input)
        {
            if (input.Attachments != null && input.Attachments.Any())
            {
                var order = Repository.Get(input.OrderId);
                if (order != null)
                {
                    order.Status = (int)OrderStatus.DesignApproved; // Set default status to 0 (Pending)
                    Repository.Update(order);
                }

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
            }
        }


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

            // Update the status to "Printed Test"
            order.Status = (int)OrderStatus.PrintingTest;

            // Save the changes
            await Repository.UpdateAsync(order);
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

            // Update the status to "Test Printed Confirmed"
            order.Status = (int)OrderStatus.PrintingTestConfirmed;

            // Save the changes
            await Repository.UpdateAsync(order);
        }

        public async Task PerformPrintingAsync(int id)
        {
            // Retrieve the order by ID
            var order = await Repository.GetAsync(id);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {id} not found", nameof(id));
            }

            // Update the status to "Printing"
            order.Status = (int)OrderStatus.Printing;

            // Save the changes
            await Repository.UpdateAsync(order);
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

            // Update the status to "Processing"
            order.Status = (int)OrderStatus.Processing;

            // Save the changes
            await Repository.UpdateAsync(order);
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

            // Update the status to "Shipped"
            order.Status = (int)OrderStatus.Delivering;

            // Save the changes
            await Repository.UpdateAsync(order);
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

            // Update the status to "Completed"
            order.Status = (int)OrderStatus.Completed;

            // Save the changes
            await Repository.UpdateAsync(order);
        }




    }
}
