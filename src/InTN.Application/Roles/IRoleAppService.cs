using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.Roles.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.Roles;

public interface IRoleAppService : IAsyncCrudAppService<RoleDto, int, PagedRoleResultRequestDto, CreateRoleDto, RoleDto>
{
    Task<ListResultDto<PermissionDto>> GetAllPermissions();

    Task<GetRoleForEditOutput> GetRoleForEdit(EntityDto input);
    Task<List<int>> GetRoleIdsByUserIdAsync(long userId);
    Task<ListResultDto<RoleListDto>> GetRolesAsync(GetRolesInput input);


}
