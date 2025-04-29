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

        public async Task CreateQuotation([FromForm]  QuotationDto input)
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
    }
}
