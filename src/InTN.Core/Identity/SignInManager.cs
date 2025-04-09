﻿using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Uow;
using InTN.Authorization.Roles;
using InTN.Authorization.Users;
using InTN.MultiTenancy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InTN.Identity;

public class SignInManager : AbpSignInManager<Tenant, Role, User>
{
    public SignInManager(
        UserManager userManager,
        IHttpContextAccessor contextAccessor,
        UserClaimsPrincipalFactory claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<User>> logger,
        IUnitOfWorkManager unitOfWorkManager,
        ISettingManager settingManager,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<User> userConfirmation)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, unitOfWorkManager, settingManager, schemes, userConfirmation)
    {
    }
}
