using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTN.Products.Dto
{
    public class SelectProductItemDto : EntityDto<int>
    {
        public string Text { get; set; }
    }
}
