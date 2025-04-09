using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using InTN.EntityFrameworkCore;
using InTN.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace InTN.Web.Tests;

[DependsOn(
    typeof(InTNWebMvcModule),
    typeof(AbpAspNetCoreTestBaseModule)
)]
public class InTNWebTestModule : AbpModule
{
    public InTNWebTestModule(InTNEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
    }

    public override void PreInitialize()
    {
        Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(InTNWebTestModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<ApplicationPartManager>()
            .AddApplicationPartsIfNotAddedBefore(typeof(InTNWebMvcModule).Assembly);
    }
}