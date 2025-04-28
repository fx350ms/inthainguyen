using Abp.Application.Services.Dto;
using Abp.Application.Services;
using InTN.IdentityCodes.Dto;

namespace InTN.IdentityCodes
{
    public interface IIdentityCodeAppService : IAsyncCrudAppService<IdentityCodeDto, long, PagedResultRequestDto, IdentityCodeDto, IdentityCodeDto>
    {
    }
}
