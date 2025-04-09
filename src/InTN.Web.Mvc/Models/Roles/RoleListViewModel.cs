using InTN.Roles.Dto;
using System.Collections.Generic;

namespace InTN.Web.Models.Roles;

public class RoleListViewModel
{
    public IReadOnlyList<PermissionDto> Permissions { get; set; }
}
