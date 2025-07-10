using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.Authorization.Users;
using InTN.Roles.Dto;
using InTN.Users.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.Users;

public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
{
    Task DeActivate(EntityDto<long> user);
    Task Activate(EntityDto<long> user);
    Task<ListResultDto<RoleDto>> GetRoles();
    Task ChangeLanguage(ChangeUserLanguageDto input);

    Task<bool> ChangePassword(ChangePasswordDto input);
    Task<List<User>> GetByRoleIds(List<long> roleIds);
    Task<List<UserIdentifier>> GetAllListUserIdentifierByRoleIdsAsync(List<int> roleIds);
    Task<List<UserIdentifier>> GetAllListUserIdentifierAsync();
}
