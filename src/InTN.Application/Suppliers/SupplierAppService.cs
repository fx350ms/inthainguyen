
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.Suppliers.Dto;
using InTN.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.Suppliers
{
    public class SupplierAppService : AsyncCrudAppService<Supplier, SupplierDto, int, PagedResultRequestDto, SupplierDto, SupplierDto>, ISupplierAppService
    {
        public SupplierAppService(IRepository<Supplier> repository) : base(repository)
        {
        }

        public async Task<List<SupplierDto>> GetAllListAsync()
        {
            var data = await Repository.GetAllListAsync();
            var result = ObjectMapper.Map<List<SupplierDto>>(data);
            return result;
        }
    }
}