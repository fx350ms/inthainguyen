using Abp.Application.Services;
using Abp.Domain.Repositories;
using InTN.Entities;
using System.Threading.Tasks;

namespace InTN.Transactions
{
    public class TransactionAppService : ApplicationService
    {
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IRepository<Customer> _customerRepository;

        public TransactionAppService(
            IRepository<Transaction> transactionRepository,
            IRepository<Customer> customerRepository)
        {
            _transactionRepository = transactionRepository;
            _customerRepository = customerRepository;
        }

        public async Task CreateTransactionAsync(int customerId, decimal amount, string description, int? orderId = null)
        {
            // Tạo giao dịch mới
            var transaction = new Transaction
            {
                CustomerId = customerId,
                Amount = amount,
                Description = description,
                OrderId = orderId
            };

            await _transactionRepository.InsertAsync(transaction);

            // Cập nhật công nợ của khách hàng
            var customer = await _customerRepository.GetAsync(customerId);
            customer.TotalDebt += amount; // Tăng/giảm công nợ
            await _customerRepository.UpdateAsync(customer);
        }
    }
}