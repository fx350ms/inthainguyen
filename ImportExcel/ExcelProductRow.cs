namespace ImportExcel
{
    public class ExcelProductRow
    {
        public string ProductTypeName { get; set; }
        public string ProductCategory { get; set; } // Hierarchical categories
        public string Code { get; set; }
        public string Name { get; set; }
        public string PriceBeforeTax { get; set; } // parse to decimal
        public string Properties { get; set; } // parse later
        public string ImageUrls { get; set; } // split later
        public string Description { get; set; }
        public string InvoiceNote { get; set; }
    }
}