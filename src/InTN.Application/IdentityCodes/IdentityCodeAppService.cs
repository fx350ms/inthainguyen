using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using InTN.Entities;
using InTN.IdentityCodes.Dto;
using InTN.IdentityCodes;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace InTN.Customers
{

    public class IdentityCodeAppService : AsyncCrudAppService<IdentityCode, IdentityCodeDto, long, PagedResultRequestDto, IdentityCodeDto, IdentityCodeDto>, IIdentityCodeAppService
    {
        public IdentityCodeAppService(IRepository<IdentityCode,long> repository)
            : base(repository)
        {

        }


        public async Task<IdentityCodeDto> GenerateNewSequentialNumberAsync(string prefix)
        {
            long currentDate = Convert.ToInt64(DateTime.Now.ToString("yyMMdd"));
            var latestRecord = await Repository.GetAll()
                .Where(ic => ic.Prefix == prefix && ic.Date == currentDate)
                .OrderByDescending(ic => ic.Id)
                .FirstOrDefaultAsync();

            long newSequentialNumber = 1;
            if (latestRecord != null)
            {
                newSequentialNumber = latestRecord.SequentialNumber + 1;
            }

            var newRecord = new IdentityCodeDto
            {
                Date = currentDate,
                Prefix = prefix,
                SequentialNumber = newSequentialNumber
            };

            await base.CreateAsync(newRecord);
            await CurrentUnitOfWork.SaveChangesAsync();

            return newRecord;
        }

    }
}
