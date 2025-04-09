using Abp.Application.Services;
using InTN.Sessions.Dto;
using System.Threading.Tasks;

namespace InTN.Sessions;

public interface ISessionAppService : IApplicationService
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
}
