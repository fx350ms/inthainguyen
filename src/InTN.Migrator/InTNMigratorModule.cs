using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using InTN.Configuration;
using InTN.EntityFrameworkCore;
using InTN.Migrator.DependencyInjection;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;

namespace InTN.Migrator;

[DependsOn(typeof(InTNEntityFrameworkModule))]
public class InTNMigratorModule : AbpModule
{
    private readonly IConfigurationRoot _appConfiguration;

    public InTNMigratorModule(InTNEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

        _appConfiguration = AppConfigurations.Get(
            typeof(InTNMigratorModule).GetAssembly().GetDirectoryPathOrNull()
        );
    }

    public override void PreInitialize()
    {
        Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
            InTNConsts.ConnectionStringName
        );

        Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        Configuration.ReplaceService(
            typeof(IEventBus),
            () => IocManager.IocContainer.Register(
                Component.For<IEventBus>().Instance(NullEventBus.Instance)
            )
        );
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(InTNMigratorModule).GetAssembly());
        ServiceCollectionRegistrar.Register(IocManager);
    }
}
