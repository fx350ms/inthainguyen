using InTN.Roles.Dto;
using System.Collections.Generic;

namespace InTN.Web.Models.Common;

public interface IPermissionsEditViewModel
{
    List<FlatPermissionDto> Permissions { get; set; }
}