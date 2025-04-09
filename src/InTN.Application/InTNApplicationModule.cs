using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using InTN.Authorization;

namespace InTN;

[DependsOn(
    typeof(InTNCoreModule),
    typeof(AbpAutoMapperModule))]
public class InTNApplicationModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Authorization.Providers.Add<InTNAuthorizationProvider>();
    }

    public override void Initialize()
    {
        var thisAssembly = typeof(InTNApplicationModule).GetAssembly();

        IocManager.RegisterAssemblyByConvention(thisAssembly);

        Configuration.Modules.AbpAutoMapper().Configurators.Add(
            // Scan the assembly for classes which inherit from AutoMapper.Profile
            cfg => cfg.AddMaps(thisAssembly)
        );
    }
}
