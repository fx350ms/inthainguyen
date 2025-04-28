using Abp.Application.Services.Dto;

namespace InTN.IdentityCodes.Dto
{
    public class IdentityCodeDto :  EntityDto<long>
    {
        public long Date { get; set; }
        public string Prefix { get; set; }
        public long SequentialNumber { get; set; }
    }
}
