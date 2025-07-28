using Abp.AutoMapper;
using Abp.Localization;
using Abp.Modules;
using Abp.Reflection.Extensions;
using InTN.Authorization;
using InTN.Brands.Dto;
using InTN.Customers.Dto;
using InTN.Entities;
using InTN.FileUploads.Dto;
using InTN.IdentityCodes.Dto;
using InTN.OrderAttachments.Dto;
using InTN.OrderLogs.Dto;
using InTN.Orders.Dto;
using InTN.Printers.Dto;
using InTN.Processes.Dto;
using InTN.ProductCategories.Dto;
using InTN.ProductNotes.Dto;
using InTN.ProductPriceCombinations.Dto;
using InTN.ProductProperties.Dto;
using InTN.Products.Dto;
using InTN.ProductTypes.Dto;
using InTN.Suppliers.Dto;
using InTN.Transactions.Dto;

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
            cfg.CreateMap<Transaction, TransactionDto>().ReverseMap();
            cfg.CreateMap<Transaction, CreateTransactionDto>().ReverseMap();
            cfg.CreateMap<TransactionDto, CreateTransactionDto>().ReverseMap();

            cfg.CreateMap<ProductCategory, ProductCategoryDto>().ReverseMap();
            cfg.CreateMap<ProductProperty, ProductPropertyDto>().ReverseMap();
            cfg.CreateMap<Supplier, SupplierDto>().ReverseMap();
            cfg.CreateMap<Brand, BrandDto>().ReverseMap();
            cfg.CreateMap<ProductType, ProductTypeDto>().ReverseMap();

            cfg.CreateMap<FileUpload, FileUploadDto>().ReverseMap();
            cfg.CreateMap<FileUpload, CreateFileUploadDto>().ReverseMap();

            cfg.CreateMap<Product, ProductDto>().ReverseMap();
            cfg.CreateMap<Product, ProductWithImageDto>().ReverseMap();
            cfg.CreateMap<Product, CreateProductDto>().ReverseMap();
            cfg.CreateMap<ProductPriceCombination, ProductPriceCombinationDto>().ReverseMap();

            cfg.CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
            cfg.CreateMap<OrderDetail, OrderDetailViewDto>().ReverseMap();
            cfg.CreateMap<ProductNote, ProductNoteDto>().ReverseMap();

            cfg.CreateMap<Process, ProcessDto>().ReverseMap();
            cfg.CreateMap<ProcessStep, ProcessStepDto>().ReverseMap();
            cfg.CreateMap<ProcessStepGroup, ProcessStepGroupDto>().ReverseMap();

            cfg.CreateMap<Printer, PrinterDto>().ReverseMap();
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
