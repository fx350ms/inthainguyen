using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.Suppliers.Dto;
using InTN.Entities;

namespace InTN.Suppliers
{
    public class SupplierAppService : AsyncCrudAppService<Supplier, SupplierDto, int, PagedResultRequestDto, SupplierDto, SupplierDto>, ISupplierAppService
    {
        public SupplierAppService(IRepository<Supplier> repository) : base(repository)
        {
        }
    }
}