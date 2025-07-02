
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.CodeAnalysis;

namespace ImportExcel
{

    public class ExcelReader
    {
        public List<ExcelProductRow> ReadExcel(string filePath)
        {
            var rows = new List<ExcelProductRow>();

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false))
            {
                WorkbookPart workbookPart = document.WorkbookPart;
                Sheet sheet = workbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
                WorksheetPart worksheetPart = (WorksheetPart)(workbookPart.GetPartById(sheet.Id));
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

                var sharedStringTable = workbookPart.SharedStringTablePart.SharedStringTable;

                var allRows = sheetData.Elements<Row>().ToList();
                if (allRows.Count < 2) return rows; // No data

                foreach (var row in allRows.Skip(1)) // Skip header
                {
                    var cellValues = row.Elements<Cell>().Select(c => GetCellValue(c, sharedStringTable)).ToList();

                    if (cellValues.Count < 9) continue; // ensure data integrity

                    var product = new ExcelProductRow
                    {
                        ProductTypeName = cellValues[0]?.Trim(),
                        ProductCategory = cellValues[1]?.Trim(),
                        Code = cellValues[2]?.Trim(),
                        Name = cellValues[3]?.Trim(),
                        PriceBeforeTax = cellValues[4],
                        Properties = cellValues[5]?.Trim(),
                        ImageUrls = cellValues[6]?.Trim(),
                        Description = cellValues[7]?.Trim(),
                        InvoiceNote = cellValues[8]?.Trim()
                    };

                    rows.Add(product);
                }
            }

            return rows;
        }

        private string GetCellValue(Cell cell, SharedStringTable sharedStringTable)
        {
            if (cell == null || cell.CellValue == null) return string.Empty;
            var value = cell.CellValue.InnerText;
            if ((cell.DataType != null && cell.DataType.Value == CellValues.SharedString))
            {
                return sharedStringTable.ElementAt(int.Parse(value)).InnerText;
            }
            return value;
        }
    }
}