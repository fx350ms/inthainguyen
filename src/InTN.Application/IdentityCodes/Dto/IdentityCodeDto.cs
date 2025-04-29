using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace InTN.IdentityCodes.Dto
{
    public class IdentityCodeDto :  EntityDto<long>
    {
        public long Date { get; set; }
        [StringLength(10)]
        public string Prefix { get; set; }
        public long SequentialNumber { get; set; }

        public string Code => $"{Prefix}{Date}{SequentialNumber:D2}";
    }
}
