using Abp.Application.Services;
using InTN.MultiTenancy.Dto;

namespace InTN.MultiTenancy;

public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
{
}

