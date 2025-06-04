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
        private readonly IRepository<Product> _productRepository;
        private readonly IFileUploadAppService _fileUploadAppService;
        public ProductAppService(IRepository<Product> repository,
            IFileUploadAppService fileUploadAppService  // Thêm dịch vụ FileUpload nếu cần thiết, có thể để null nếu không sử dụng trong service này
            )
            : base(repository)
        {
            _productRepository = repository;
            _fileUploadAppService = fileUploadAppService; // Khởi tạo dịch vụ FileUpload

        }

        public async Task<ProductDto> GetProductDetailsAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);
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
    }
}