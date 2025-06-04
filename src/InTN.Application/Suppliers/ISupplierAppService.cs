using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.Suppliers.Dto;

namespace InTN.Suppliers
{
    public interface ISupplierAppService : IAsyncCrudAppService<SupplierDto, int, PagedResultRequestDto, SupplierDto, SupplierDto>
    {
    }
}