using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.Runtime;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;


namespace InTN.Authorization
{
    public class IntnAppSession : ClaimsAbpSession, ITransientDependency
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IntnAppSession(
            IPrincipalAccessor principalAccessor,
            IMultiTenancyConfig multiTenancy,
            ITenantResolver tenantResolver,
            IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider

            ) :
            base(principalAccessor, multiTenancy, tenantResolver, sessionOverrideScopeProvider)
        {
        }
 
        public string UserName
        {
            get
            {
                var claim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                return claim == null ? string.Empty : claim.Value;
            }
        }


         
    }
}
