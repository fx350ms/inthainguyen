using Abp.Application.Services.Dto;

namespace InTN.Printers.Dto
{
    public class PrinterDto : EntityDto<int>
    {
        public string Name { get; set; } // Tên máy in
        public string Description { get; set; } // Mô tả máy in
    }

    public class CreatePrinterDto
    {
        public string Name { get; set; } // Tên máy in
        public string Description { get; set; } // Mô tả máy in
    }

    public class UpdatePrinterDto : CreatePrinterDto
    {
        public int Id { get; set; } // ID máy in
    }
}