using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace InTN.Controllers
{
    public abstract class InTNControllerBase : AbpController
    {
        protected InTNControllerBase()
        {
            LocalizationSourceName = InTNConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
