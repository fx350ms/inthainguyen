using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using InTN.Entities;
using InTN.IdentityCodes.Dto;
using InTN.IdentityCodes;

namespace InTN.Customers
{

    public class IdentityCodeAppService : AsyncCrudAppService<IdentityCode, IdentityCodeDto, long, PagedResultRequestDto, IdentityCodeDto, IdentityCodeDto>, IIdentityCodeAppService
    {
        public IdentityCodeAppService(IRepository<IdentityCode,long> repository)
            : base(repository)
        {

        }
    }
}
