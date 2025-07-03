using System.Text.Json;
using DocumentFormat.OpenXml.VariantTypes;
using InTN.Entities;
using InTN.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;


namespace ImportExcel
{
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
            List<CombinationItem> combinations = new List<CombinationItem>();
            List<CombinationWithPrice> combinationWithPrices = new List<CombinationWithPrice>();
            foreach (var row in rows)
            {
                if (lastRow != null && row.Name != lastRow.Name)
                {
                    await SaveProductWithCombinations(bufferedProduct!, combinationWithPrices);
                    bufferedProduct = null;
                    combinations = new List<CombinationItem>();
                    combinationWithPrices = new List<CombinationWithPrice>();
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
                        FileUploadIds = imageIds.Count > 0 ? System.Text.Json.JsonSerializer.Serialize(imageIds) : null,
                        Price = price,
                        IsActive = true
                    };
                }

                combinations = await CreatePropertyMatrix(row.Properties);
              
                if (combinations != null && combinations.Count > 0)
                {
                    combinationWithPrices.Add(new CombinationWithPrice
                    {
                        Price = price,
                        Combinations = combinations
                    });
                    combinations = new List<CombinationItem>(); // Reset for next row
                }

                lastRow = row;
            }

            if (bufferedProduct != null)
            {
                await SaveProductWithCombinations(bufferedProduct, combinationWithPrices);
                combinations = new List<CombinationItem>();
                combinationWithPrices = new List<CombinationWithPrice>();
            }
        }

        public async Task<List<CombinationItem>> CreatePropertyMatrix(string properties)
        {
            var result = new List<CombinationItem>();
            if (string.IsNullOrWhiteSpace(properties)) return result;

            var pairs = properties.Split('|');
            var valueLists = new List<List<(int PropertyId, string PropertyName, string Value)>>();

            foreach (var pair in pairs)
            {
                var kv = pair.Split(':');
                if (kv.Length != 2) continue;
                var propName = kv[0].Trim();
                var rawValue = kv[1].Trim();

                if (!_propertyCache.TryGetValue(propName, out var propId))
                {
                    var existing = await _db.ProductProperties.FirstOrDefaultAsync(x => x.Name == propName);
                    if (existing != null) propId = existing.Id;
                    else
                    {
                        var prop = new ProductProperty { Name = propName };
                        _db.ProductProperties.Add(prop);
                        await _db.SaveChangesAsync();
                        propId = prop.Id;
                    }
                    _propertyCache[propName] = propId;
                }

                var item = new CombinationItem()
                {
                    PropertyId = propId,
                    PropertyName = propName,
                    Value = rawValue
                };
                result.Add(item);
            }

     
            return result;
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


        private async Task SaveProductWithCombinations(Product product, List<CombinationWithPrice> combinationWithPrices)
        {

            // Lưu thuộc tính của sản phẩm
            var properties = combinationWithPrices
                .SelectMany(c => c.Combinations)
                .GroupBy(c => c.PropertyId)
                .Select(g => new 
                {
                    PropertyId = g.Key,
                    PropertyName = g.First().PropertyName,
                    Values = g.Select(c => c.Value).Distinct().ToList()
                })
                .ToList();

            product.Properties = JsonSerializer.Serialize(properties);

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            var comb = new ProductPriceCombination
            {
                ProductId = product.Id,
                PriceCombination = JsonSerializer.Serialize(combinationWithPrices)
            };

            _db.ProductPriceCombinations.Add(comb);

            await _db.SaveChangesAsync();
        }


    }
}