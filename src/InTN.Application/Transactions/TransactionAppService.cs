using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using InTN.Entities;
using InTN.Transactions.Dto;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace InTN.Transactions
{
    public class TransactionAppService : AsyncCrudAppService<Transaction, TransactionDto, int, PagedResultRequestDto, TransactionDto, TransactionDto>, ITransactionAppService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerBalanceHistory> _customerBalanceHistoryRepository;
        public TransactionAppService(IRepository<Transaction> repository,
            IRepository<Order> orderRepository,
            IRepository<Customer> customerRepository,
            IRepository<CustomerBalanceHistory> customerBalanceHistoryRepository
            )
            : base(repository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _customerBalanceHistoryRepository = customerBalanceHistoryRepository;
        }

 
        //public async Task CreateTransactionAsync(int customerId, decimal amount, string description, int? orderId = null)
        //{
        //    // Tạo giao dịch mới
        //    var transaction = new Transaction
        //    {
        //        CustomerId = customerId,
        //        Amount = amount,
        //        Description = description,
        //        OrderId = orderId
        //    };

        //    await _transactionRepository.InsertAsync(transaction);

        //    // Cập nhật công nợ của khách hàng
        //    var customer = await _customerRepository.GetAsync(customerId);
        //    customer.TotalDebt += amount; // Tăng/giảm công nợ
        //    await _customerRepository.UpdateAsync(customer);
        //}

        [HttpPost]
        public async Task<TransactionDto> CreateWithAttachmentAsync([FromForm] TransactionDto input)
        {
            if(input.TransactionType == (int) TransactionType.OrderPayment && input.OrderId == null)
            {
                throw new UserFriendlyException("Vui lòng chọn đơn hàng");
            }

            if(input.TransactionType == (int) TransactionType.DebtPayment && input.CustomerId == null)
            {
                throw new UserFriendlyException("Vui lòng chọn khách hàng");
            }

            if (input.Attachments != null && input.Attachments.Count > 0)
            {
                var file = input.Attachments[0];
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream); // Đọc dữ liệu từ file
                    input.FileContent = memoryStream.ToArray(); // Chuyển đổi dữ liệu thành mảng byte
                }
            }
            // Tạo giao dịch mới
            var result=  await base.CreateAsync(input);

            if(input.TransactionType == (int)TransactionType.OrderPayment)
            {
                var order = await _orderRepository.GetAsync(input.OrderId.Value);
                order.PaymentStatus = (int)OrderPaymentStatus.Paid;
                await _orderRepository.UpdateAsync(order);
            }

            if (input.TransactionType == (int)TransactionType.DebtPayment)
            {
                var customer = await _customerRepository.GetAsync(input.CustomerId.Value);
                customer.TotalDebt -= input.Amount; // Tăng/giảm công nợ
                await _customerRepository.UpdateAsync(customer);

                var customerBalanceHistory = new CustomerBalanceHistory
                {
                    CustomerId = input.CustomerId.Value,
                    Amount = input.Amount,
                    TransactionId = result.Id,
                    BalanceAfterTransaction = customer.TotalDebt,
                    Type = (int) DebtType.Decrease  
                };
                await _customerBalanceHistoryRepository.InsertAsync(customerBalanceHistory);
            }

            return result;
        }
 
    }
}