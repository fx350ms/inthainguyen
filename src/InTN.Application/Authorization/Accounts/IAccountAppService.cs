using Abp.Application.Services;
using InTN.Authorization.Accounts.Dto;
using System.Threading.Tasks;

namespace InTN.Authorization.Accounts;

public interface IAccountAppService : IApplicationService
{
    Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

    Task<RegisterOutput> Register(RegisterInput input);
}
