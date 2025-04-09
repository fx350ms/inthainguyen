using InTN.Roles.Dto;
using System.Collections.Generic;

namespace InTN.Web.Models.Users;

public class UserListViewModel
{
    public IReadOnlyList<RoleDto> Roles { get; set; }
}
