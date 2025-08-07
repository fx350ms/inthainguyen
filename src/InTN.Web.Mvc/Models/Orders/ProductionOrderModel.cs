using InTN.Orders.Dto;
using InTN.Printers.Dto;
using InTN.Processes.Dto;
using System.Collections.Generic;

namespace InTN.Web.Models.Orders
{
    public class ProductionOrderModel
    {
        public OrderDto Order { get; set; }

        public List<PrinterDto> Printers { get; set; } = new List<PrinterDto>();

        public List<ProcessStepGroupDto> ProcessStepGroups { get; set; } = new List<ProcessStepGroupDto>();
    }
}
