using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.ProductPriceCombinations.Dto;
using InTN.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Abp;
using System;
using System.Linq;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InTN.ProductPriceCombinations
{
    public class ProductPriceCombinationAppService : AsyncCrudAppService<
        ProductPriceCombination, // Entity chính
        ProductPriceCombinationDto, // DTO chính
        int,                        // Kiểu dữ liệu của khóa chính
        PagedResultRequestDto,      // DTO cho phân trang
        ProductPriceCombinationDto, // DTO cho tạo mới
        ProductPriceCombinationDto>, // DTO cho cập nhật
        IProductPriceCombinationAppService
    {

        private readonly IRepository<Product> _productRepository;

        public ProductPriceCombinationAppService(IRepository<ProductPriceCombination> repository,
            IRepository<Product> productRepository)
            : base(repository)
        {
            _productRepository = productRepository;
        }



        public async Task<List<ProductPriceCombinationDto>> GetAllProductPriceCombinationsAsync()
        {
            var productPriceCombinations = await Repository.GetAllAsync();
            return ObjectMapper.Map<List<ProductPriceCombinationDto>>(productPriceCombinations);
        }

        public async Task SavePriceCombinations(SavePriceCombinationsDto input)
        {
            var product = await _productRepository.GetAsync(input.ProductId);
            if (product == null)
            {
                throw new AbpException($"Product with ID {input.ProductId} not found.");
            }

            product.Properties = JsonConvert.SerializeObject(input.Properties);
            await _productRepository.UpdateAsync(product);

            var itemExisted = await Repository.FirstOrDefaultAsync(x => x.ProductId == input.ProductId);
            if (itemExisted != null)
            {
                // Update bản ghi cũ
                itemExisted.PriceCombination = JsonConvert.SerializeObject(input.PriceCombinations);
                await Repository.UpdateAsync(itemExisted);
            }
            else
            {
                // Tạo mới bản ghi
                var newItem = new ProductPriceCombination
                {
                    ProductId = input.ProductId,
                    PriceCombination = JsonConvert.SerializeObject(input.PriceCombinations)
                };
                await Repository.InsertAsync(newItem);
            }



        }

        public async Task<List<PriceCombinationDto>> GetPriceCombinationsByProductIdAsync(int productId)
        {
            var item = await Repository.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (item != null && !string.IsNullOrEmpty(item.PriceCombination))
            {
                return JsonConvert.DeserializeObject<List<PriceCombinationDto>>(item.PriceCombination) ?? new List<PriceCombinationDto>();
            }
            return null;
        }


        public async Task<decimal> CalculatePriceAsync(int productId, List<PropertyWithSelectedValueDto> selectedProperties)
        {
            var product = await _productRepository.GetAsync(productId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            // Logic tính giá dựa trên thuộc tính
            decimal basePrice = 0;

            var productProperties = JsonConvert.DeserializeObject<List<PropertyWithValuesDto>>(product.Properties) ?? new List<PropertyWithValuesDto>();
            //if (productProperties.Count != selectedProperties.Count)
            //{
            //   return product.Price ?? 0; // Trả về giá gốc nếu số lượng thuộc tính không khớp
            //}

            var priceCombinations = await GetPriceCombinationsByProductIdAsync(productId);

            foreach (var selectedProperty in selectedProperties)
            {
                var id = selectedProperty.PropertyId;
                var value = selectedProperty.Value;
                priceCombinations = priceCombinations
                    .Where(u => u.Combinations.Any(v => v.PropertyId == id && v.Value == value)).ToList();
            }
            if (priceCombinations != null && priceCombinations.Count > 0)
            {
                // Nếu có các kết hợp giá, lấy giá của kết hợp đầu tiên
                basePrice = priceCombinations.First().Price;
            }
            else
            {
                // Nếu không có kết hợp nào, sử dụng giá gốc của sản phẩm
                basePrice = product.Price ?? 0;
            }

            return basePrice;
        }
    }
}