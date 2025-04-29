using Abp.Application.Services.Dto;
using Abp.Application.Services;
using InTN.IdentityCodes.Dto;
using System.Threading.Tasks;

namespace InTN.IdentityCodes
{
    public interface IIdentityCodeAppService : IAsyncCrudAppService<IdentityCodeDto, long, PagedResultRequestDto, IdentityCodeDto, IdentityCodeDto>
    {
       
        public Task<IdentityCodeDto> GenerateNewSequentialNumberAsync(string prefix);
    }
}
