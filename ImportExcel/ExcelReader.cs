using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using InTN.Entities;
using InTN.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


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

    public class ProductImporter
    {
        private readonly InTNDbContext _db;
        private readonly Dictionary<string, int> _categoryCache = new();
        private readonly Dictionary<string, int> _typeCache = new();
        private readonly Dictionary<string, int> _propertyCache = new();

        public ProductImporter(InTNDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task ImportAsync(List<ExcelProductRow> rows)
        {
            ExcelProductRow? lastRow = null;
            Product? bufferedProduct = null;
            List<ProductPriceCombination> bufferedCombinations = new();

            foreach (var row in rows)
            {
                if (lastRow != null && row.Name != lastRow.Name)
                {
                    await SaveProductWithCombinations(bufferedProduct!, bufferedCombinations);
                    bufferedProduct = null;
                    bufferedCombinations.Clear();
                }

                var productTypeId = await GetOrCreateProductTypeAsync(row.ProductTypeName);
                var categoryId = await GetOrCreateProductCategoryAsync(row.ProductCategory);
                var imageIds = await DownloadImagesAsync(row.ImageUrls);
                var price = decimal.TryParse(row.PriceBeforeTax, out var p) ? p : 0m;

                if (bufferedProduct == null)
                {
                    bufferedProduct = new Product
                    {
                        Code = row.Code,
                        Name = row.Name,
                        ProductTypeId = productTypeId,
                        ProductCategoryId = categoryId,
                        Description = row.Description,
                        InvoiceNote = row.InvoiceNote,
                        FileUploadIds = imageIds.Count > 0 ? string.Join(",", imageIds) : null,
                        Price = null, // Price stored in combinations
                        IsActive = true
                    };
                }

                var combination = await CreatePriceCombination(row.Properties, price);
                if (combination != null)
                {
                    bufferedCombinations.Add(combination);
                }

                lastRow = row;
            }

            if (bufferedProduct != null)
            {
                await SaveProductWithCombinations(bufferedProduct, bufferedCombinations);
            }
        }

        private async Task<int> GetOrCreateProductTypeAsync(string name)
        {
            if (_typeCache.TryGetValue(name, out var id)) return id;
            var existing = await _db.ProductTypes.FirstOrDefaultAsync(x => x.Name == name);
            if (existing != null) return _typeCache[name] = existing.Id;

            var newType = new ProductType { Name = name };
            _db.ProductTypes.Add(newType);
            await _db.SaveChangesAsync();
            return _typeCache[name] = newType.Id;
        }

        private async Task<int> GetOrCreateProductCategoryAsync(string path)
        {
            if (_categoryCache.TryGetValue(path, out var id)) return id;

            var levels = path.Split("»", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            int? parentId = null;
            string fullPath = "";
            foreach (var level in levels)
            {
                fullPath += (fullPath == "" ? "" : "»") + level;
                if (_categoryCache.TryGetValue(fullPath, out var cachedId))
                {
                    parentId = cachedId;
                    continue;
                }

                var existing = await _db.ProductCategories.FirstOrDefaultAsync(x => x.Name == level && x.ParentId == parentId);
                if (existing != null)
                {
                    _categoryCache[fullPath] = existing.Id;
                    parentId = existing.Id;
                    continue;
                }

                var newCat = new ProductCategory { Name = level, ParentId = parentId };
                _db.ProductCategories.Add(newCat);
                await _db.SaveChangesAsync();
                _categoryCache[fullPath] = newCat.Id;
                parentId = newCat.Id;
            }

            return parentId ?? 0;
        }

        private async Task<List<int>> DownloadImagesAsync(string imageUrls)
        {
            var ids = new List<int>();
            if (string.IsNullOrWhiteSpace(imageUrls)) return ids;

            foreach (var url in imageUrls.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                try
                {
                    using var http = new HttpClient();
                    var data = await http.GetByteArrayAsync(url);
                    var file = new FileUpload
                    {
                        FileName = Path.GetFileName(url),
                        FileType = "image/jpeg",
                        FileSize = data.Length,
                        FileContent = data,
                        Type = 0
                    };
                    _db.FileUploads.Add(file);
                    await _db.SaveChangesAsync();
                    ids.Add(file.Id);
                }
                catch
                {
                    // skip failed download
                }
            }

            return ids;
        }

        private async Task<ProductPriceCombination?> CreatePriceCombination(string properties, decimal price)
        {
            if (string.IsNullOrWhiteSpace(properties)) return null;

            var pairs = properties.Split('|');
            var items = new List<object>();
            foreach (var pair in pairs)
            {
                var kv = pair.Split(':');
                if (kv.Length != 2) continue;
                var propName = kv[0].Trim();
                var value = kv[1].Trim();

                if (!_propertyCache.TryGetValue(propName, out var propId))
                {
                    var existing = await _db.ProductProperties.FirstOrDefaultAsync(x => x.Name == propName);
                    if (existing != null)
                    {
                        propId = existing.Id;
                    }
                    else
                    {
                        var prop = new ProductProperty { Name = propName };
                        _db.ProductProperties.Add(prop);
                        await _db.SaveChangesAsync();
                        propId = prop.Id;
                    }
                    _propertyCache[propName] = propId;
                }

                items.Add(new { PropertyId = propId, PropertyName = propName, Value = value });
            }

            var json = System.Text.Json.JsonSerializer.Serialize(new { Combination = items, Price = price });

            return new ProductPriceCombination
            {
                PriceCombination = json
            };
        }

        private async Task SaveProductWithCombinations(Product product, List<ProductPriceCombination> combinations)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            foreach (var comb in combinations)
            {
                comb.ProductId = product.Id;
                _db.ProductPriceCombinations.Add(comb);
            }

            await _db.SaveChangesAsync();
        }
    }
}