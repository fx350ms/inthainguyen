using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.Products.Dto;
using InTN.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Json;
using System.IO;
using System.Linq;
using InTN.FileUploads;
using InTN.FileUploads.Dto;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using InTN.Commons;
using InTN.ProductPriceCombinations.Dto;
using Abp;
using Newtonsoft.Json;

namespace InTN.Products
{
    public class ProductAppService : AsyncCrudAppService<
        Product, // Entity chính
        ProductDto, // DTO chính
        int,        // Kiểu dữ liệu của khóa chính
        PagedResultRequestDto, // DTO cho phân trang
        CreateProductDto,      // DTO cho tạo mới
        ProductDto>,           // DTO cho cập nhật
        IProductAppService      // Interface
    {
        private readonly IFileUploadAppService _fileUploadAppService;
        public ProductAppService(IRepository<Product> repository,
            IFileUploadAppService fileUploadAppService  // Thêm dịch vụ FileUpload nếu cần thiết, có thể để null nếu không sử dụng trong service này
            )
            : base(repository)
        {
            _fileUploadAppService = fileUploadAppService; // Khởi tạo dịch vụ FileUpload

        }

        public async Task<ProductDto> GetProductDetailsAsync(int id)
        {
            var product = await Repository.GetAsync(id);
            var productDto = ObjectMapper.Map<ProductDto>(product);
            return productDto;
        }

        [HttpPost]
        public async Task<ProductDto> CreateWithUploadImageAsync([FromForm] CreateProductDto input)
        {
            var attachmentIds = await _fileUploadAppService.UploadMultiFilesAndGetIdsAsync(input.Attachments);
            input.FileUploadIds = string.Join(",", attachmentIds);
            return await CreateAsync(input);
        }


        public async Task<PagedResultDto<ProductDto>> GetProductsAsync(PagedProductResultRequestDto input)
        {
            var query = await Repository.GetAllAsync();
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                query = query.Where(p => p.Code.Contains(input.Keyword) || p.Name.Contains(input.Keyword) || p.InvoiceNote.Contains(input.Keyword));
            }
            if (input.ProductTypeId.HasValue && input.ProductTypeId > 0)
            {
                query = query.Where(p => p.ProductTypeId == input.ProductTypeId.Value);
            }
            if (input.ProductCategoryId.HasValue && input.ProductCategoryId > 0)
            {
                query = query.Where(p => p.ProductCategoryId == input.ProductCategoryId.Value);
            }
            if (input.SupplierId.HasValue && input.SupplierId > 0)
            {
                query = query.Where(p => p.SupplierId == input.SupplierId.Value);
            }
            if (input.BrandId.HasValue && input.BrandId > 0)
            {
                query = query.Where(p => p.BrandId == input.BrandId.Value);
            }
            if (input.Status > 0)
            {
                query = query.Where(p => (input.Status == (int)ProductStatus.Active && p.IsActive)
                || (input.Status == (int)ProductStatus.Active && !p.IsActive));
            }
            var totalCount = await query.CountAsync();
            var items = await query
                .Include(u => u.Brand)
                .Include(u => u.ProductCategory)
                .Include(u => u.ProductType)
                .Include(u => u.Supplier)

                .PageBy(input)
                .ToListAsync();
            var productDtos = ObjectMapper.Map<List<ProductDto>>(items);

            return new PagedResultDto<ProductDto>
            {
                TotalCount = totalCount,
                Items = productDtos
            };
        }


        [HttpGet]
        public async Task<List<OptionItemDto>> FilterAndSearchProductAsync(FilterAndSearchProductRequestDto input)
        {
            var query = await Repository.GetAllAsync();
            if (input.ProductTypeId.HasValue && input.ProductTypeId > 0)
            {
                query = query.Where(u => u.ProductTypeId.HasValue && u.ProductTypeId == input.ProductTypeId.Value);
            }

            if (input.ProductCategoryId.HasValue && input.ProductCategoryId > 0)
            {
                query = query.Where(u => u.ProductCategoryId.HasValue && u.ProductCategoryId == input.ProductCategoryId.Value);
            }

            if (input.SupplierId.HasValue && input.SupplierId > 0)
            {
                query = query.Where(u => u.SupplierId.HasValue && u.SupplierId == input.SupplierId.Value);
            }

            if (input.BrandId.HasValue && input.BrandId > 0)
            {
                query = query.Where(u => u.BrandId.HasValue && u.BrandId == input.BrandId.Value);
            }
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                query = query.Where(u => u.Code.Contains(input.Keyword) || u.Name.Contains(input.Keyword));
            }
            var items = await query.Select(u => new OptionItemDto() { id = u.Id.ToString(), text = u.Name }).ToListAsync();
            return items;

        }


        public async Task<List<PropertyWithValuesDto>> GetProductPropertiesAsync(int productId)
        {
            var product = await GetAsync(new EntityDto<int>(productId));
            if (product == null)
            {
                throw new AbpException($"Product with ID {productId} not found.");
            }

            var properties = JsonConvert.DeserializeObject<List<PropertyWithValuesDto>>(product.Properties) ?? new List<PropertyWithValuesDto>();

            return properties;

        }
    }
}