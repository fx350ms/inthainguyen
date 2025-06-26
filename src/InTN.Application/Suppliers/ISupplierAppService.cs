using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.Suppliers.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.Suppliers
{
    public interface ISupplierAppService : IAsyncCrudAppService<SupplierDto, int, PagedResultRequestDto, SupplierDto, SupplierDto>
    {
        Task<List<SupplierDto>> GetAllListAsync();
    }
}