using Abp.Application.Services.Dto;
using Abp.Application.Services;
using InTN.Orders.Dto;
using InTN.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace InTN.Orders
{
    public interface IOrderAppService : IAsyncCrudAppService<OrderDto, int, PagedResultRequestDto, OrderDto, OrderDto>
    {
        /// <summary>
        /// Create a new order
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
         Task<Order> CreateNewAsync(CreateOrderDto input);

        /// <summary>
        /// Create a new order with quotation
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
         Task CreateQuotationAsync([FromForm] OrderQuotationUploadDto input);


        /// <summary>
        /// Create a new order with design
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
         Task ApproveDesignAsync([FromForm] OrderDesignUploadDto input);

        /// <summary>
        /// Update the status of the order to "Deposited"
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
         Task UpdateStatusToDepositedAsync([FromForm] OrderDepositUploadDto input);

        /// <summary>
        /// Update the status of the order to "Printed Test"
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
         Task UpdateStatusToPrintedTestAsync(int orderId);


        /// <summary>
        /// Confirm the printed test
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
         Task ConfirmPrintedTestAsync(int orderId);


        /// <summary>
        /// Perform printing for the order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task PerformPrintingAsync(int orderId);


        /// <summary>
        /// Perform processing for the order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task PerformProcessingAsync(int orderId);

        /// <summary>
        /// Ship the order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task ShipOrderAsync(int orderId);


        /// <summary>
        /// Complete the order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task CompleteOrderAsync(int orderId);


        /// <summary>
        /// Update the status of an order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task UpdateOrderStatusAsync(int id, int nextStepId, int status);
    }
}
