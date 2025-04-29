using Abp.AutoMapper;
using Abp.Localization;
using Abp.Modules;
using Abp.Reflection.Extensions;
using InTN.Authorization;
using InTN.Customers.Dto;
using InTN.Entities;
using InTN.IdentityCodes.Dto;
using InTN.OrderAttachments.Dto;
using InTN.OrderLogs.Dto;
using InTN.Orders.Dto;

namespace InTN;

[DependsOn(
    typeof(InTNCoreModule),
    typeof(AbpAutoMapperModule))]
public class InTNApplicationModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Authorization.Providers.Add<InTNAuthorizationProvider>();
        Configuration.Localization.Languages.Add(new LanguageInfo("vi", "Tiếng Việt", isDefault: true));
        Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
        {
            cfg.CreateMap<Customer, CustomerDto>().ReverseMap();
            cfg.CreateMap<IdentityCode, IdentityCodeDto>().ReverseMap();
            cfg.CreateMap<Order, OrderDto>().ReverseMap();
            cfg.CreateMap<Order, CreateOrderDto>().ReverseMap();
            cfg.CreateMap<CreateOrderDto, OrderDto>().ReverseMap();
            cfg.CreateMap<OrderAttachment, OrderAttachmentDto>().ReverseMap();
            cfg.CreateMap<OrderLog, OrderLogDto>().ReverseMap();

        });
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
