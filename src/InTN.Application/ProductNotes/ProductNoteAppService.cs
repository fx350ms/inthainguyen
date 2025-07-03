using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.Commons;
using InTN.Entities;
using InTN.ProductCategories.Dto;
using InTN.ProductNotes.Dto;
using InTN.Products.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InTN.ProductNotes
{
    public class ProductNoteAppService : AsyncCrudAppService<
        ProductNote, ProductNoteDto, int, PagedResultRequestDto, ProductNoteDto, ProductNoteDto>, IProductNoteAppService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        public ProductNoteAppService(IRepository<ProductNote> repository,
            IRepository<ProductCategory> productCategoryRepository,
            IRepository<Product> productRepository
            ) : base(repository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
        }

        public override Task<ProductNoteDto> CreateAsync(ProductNoteDto input)
        {
            return base.CreateAsync(input);
        }

        public async Task<PagedResultDto<ProductNoteDto>> GetDataAsync(PagedProductNoteResultRequestDto input)
        {
            var query = await Repository.GetAllAsync();
            if (input.ProductCategoryId.HasValue && input.ProductCategoryId > 0)
            {
                query = query.Where(u => u.ProductCategoryId == input.ProductCategoryId);
            }

            if (input.ParentId.HasValue && input.ParentId > 0)
            {
                query = query.Where(u => u.ParentId == input.ParentId);
            }
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                query = query.Where(u => u.Note.Contains(input.Keyword));
            }
            var totalCount = await query.CountAsync();

            var items = await query

             .OrderByDescending(u => u.Id)
             .Skip(input.SkipCount)
             .Take(input.MaxResultCount)
             .ToListAsync();

            // Map the items to ProductNoteDto and include the ProductCategoryName

            var categoryIds = items.Select(x => x.ProductCategoryId).Distinct().ToList();

            var categories = await _productCategoryRepository.GetAllListAsync(x => categoryIds.Contains(x.Id));

            var data = ObjectMapper.Map<List<ProductNoteDto>>(items);

            foreach (var item in data)
            {
                var category = categories.FirstOrDefault(x => x.Id == item.ProductCategoryId);
                var categoryDto = ObjectMapper.Map<ProductCategoryDto>(category);
                item.ProductCategory = categoryDto;
                item.ProductCategoryName = categoryDto?.Name ?? string.Empty;
            }
            return new PagedResultDto<ProductNoteDto>()
            {
                TotalCount = totalCount,
                Items = data
            };

        }

       

        public async Task<List<ProductNote>> GetAllListAsync()
        {
            var data = await Repository.GetAllListAsync();
            var result = ObjectMapper.Map<List<ProductNote>>(data);
            return result;
        }

        [HttpGet]
        public async Task<List<OptionItemDto>> FilterAndSearchProductNoteAsync(FilterandSearchProductNoteRequestDto input)
        {
            var query = await Repository.GetAllAsync();
            if (input.ProductCategoryId.HasValue && input.ProductCategoryId > 0)
            {
                query = query.Where(u => u.ProductCategoryId > 0 && u.ProductCategoryId == input.ProductCategoryId);
            }

            if (!string.IsNullOrEmpty(input.Keyword))
            {
                query = query.Where(u => u.Note.Contains(input.Keyword));
            }
            var items = await query.Select(u => new OptionItemDto() { id = u.Id.ToString(), text = u.Note }).ToListAsync();
            return items;
        }

        public async Task<List<ProductNoteDto>> GetNotesByProductCategoryIdAsync(int categoryId)
        {
            var notes = await Repository.GetAllListAsync(x => x.ProductCategoryId == categoryId);
            return ObjectMapper.Map<List<ProductNoteDto>>(notes);
        }


        /// <summary>
        /// Đang dùng ở view tạo đơn hàng mới
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="onlyParent"></param>
        /// <returns></returns>
        public async Task<List<ProductNoteDto>> GetNotesByProductIdAsync(int productId, bool onlyParent = false)
        {
            var product = await _productRepository.GetAsync(productId);
            if (product != null)
            {

                var query = (await Repository.GetAllAsync())
                    .Where(x => x.ProductCategoryId == product.ProductCategoryId);
                if (onlyParent)
                {
                    query = query.Where(x => !x.ParentId.HasValue || x.ParentId == 0);
                }
                var notes = await query.Select(x => new ProductNoteDto
                {
                    Id = x.Id,
                    Note = x.Note,
                }).ToListAsync();
                return notes;
            }
            return null;
        }

        public async Task<List<ProductNoteDto>> GetNotesByParentAsync(int parentId)
        {
            var query = ( await Repository.GetAllAsync())
                .Where(x => x.ParentId == parentId);
            var notes = await query.Select(x => new ProductNoteDto
            {
                Id = x.Id,
                Note = x.Note,
            }).ToListAsync();

            return notes;
            //  return ObjectMapper.Map<List<ProductNoteDto>>(notes);
        }

    }
}