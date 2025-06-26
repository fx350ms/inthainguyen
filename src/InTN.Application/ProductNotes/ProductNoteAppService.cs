using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.Commons;
using InTN.Entities;
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
        private readonly IRepository<ProductNote> _repository;

        public ProductNoteAppService(IRepository<ProductNote> repository) : base(repository)
        {
            _repository = repository;
        }

        public override Task<ProductNoteDto> CreateAsync(ProductNoteDto input)
        {
            return base.CreateAsync(input);
        }

        public async Task<PagedResultDto<ProductNoteDto>> GetDataAsync(PagedProductNoteResultRequestDto input)
        {
            var query = await Repository.GetAllAsync();
            if (input.ProductId.HasValue && input.ProductId > 0)
            {
                query = query.Where(u => u.ProductId == input.ProductId);
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
                // .Include(x => x.Parent)
                .OrderByDescending(u => u.Id)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToListAsync();
            return new PagedResultDto<ProductNoteDto>()
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<ProductNoteDto>>(items)
            };
        }

        public async Task<List<ProductNoteDto>> GetNotesByProductIdAsync(int productId)
        {
            var notes = await _repository.GetAllListAsync(x => x.ProductId == productId);
            return ObjectMapper.Map<List<ProductNoteDto>>(notes);
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
            if (input.ProductId > 0)
            {
                query = query.Where(u => u.ProductId > 0 && u.ProductId == input.ProductId);
            }

            if (!string.IsNullOrEmpty(input.Keyword))
            {
                query = query.Where(u => u.Note.Contains(input.Keyword));
            }
            var items = await query.Select(u => new OptionItemDto() { id = u.Id.ToString(), text = u.Note }).ToListAsync();
            return items;
        }
    }
}