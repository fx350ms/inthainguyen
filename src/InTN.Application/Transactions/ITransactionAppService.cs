using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.Transactions.Dto;
using System.Threading.Tasks;

namespace InTN.Transactions
{
    public interface ITransactionAppService : IApplicationService
    {
        Task CreateTransactionAsync(TransactionDto input);

        Task<PagedResultDto<TransactionDto>> GetAllAsync(PagedResultRequestDto input);

    }
}