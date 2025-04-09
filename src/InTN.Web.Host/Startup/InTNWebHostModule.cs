using Abp.Modules;
using Abp.Reflection.Extensions;
using InTN.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace InTN.Web.Host.Startup
{
    [DependsOn(
       typeof(InTNWebCoreModule))]
    public class InTNWebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public InTNWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(InTNWebHostModule).GetAssembly());
        }
    }
}
