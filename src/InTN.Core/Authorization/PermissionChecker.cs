using Abp.Authorization;
using InTN.Authorization.Roles;
using InTN.Authorization.Users;

namespace InTN.Authorization;

public class PermissionChecker : PermissionChecker<Role, User>
{
    public PermissionChecker(UserManager userManager)
        : base(userManager)
    {
    }
}
