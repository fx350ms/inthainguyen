using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.Commons;
using InTN.Entities;
using InTN.ProductNotes.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.ProductNotes
{
    public interface IProductNoteAppService : IAsyncCrudAppService<
        ProductNoteDto, // DTO chính
        int,            // Kiểu dữ liệu của khóa chính
        PagedResultRequestDto, // DTO cho phân trang
        ProductNoteDto, // DTO cho tạo mới
        ProductNoteDto> // DTO cho cập nhật
    {
        /// <summary>
        /// Lọc và tìm kiếm ghi chú sản phẩm theo các tiêu chí nhất định.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<OptionItemDto>> FilterAndSearchProductNoteAsync(FilterandSearchProductNoteRequestDto input);

        /// <summary>
        /// Lấy tất cả ghi chú sản phẩm.
        /// </summary>
        /// <returns></returns>
        Task<List<ProductNote>> GetAllListAsync();

        /// <summary>
        /// Lấy dữ liệu ghi chú sản phẩm với phân trang và các tiêu chí lọc.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProductNoteDto>> GetDataAsync(PagedProductNoteResultRequestDto input);

        /// <summary>
        /// Lấy danh sách ghi chú sản phẩm theo ID sản phẩm.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="onlyParent"></param>
        /// <returns></returns>
        Task<List<ProductNoteDto>> GetNotesByProductIdAsync(int productId, bool onlyParent = false);

        /// <summary>
        /// Lấy danh sách ghi chú sản phẩm theo ID nhóm hàng.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<List<ProductNoteDto>> GetNotesByProductCategoryIdAsync(int categoryId);

        /// <summary>
        /// Lấy danh sách ghi chú sản phẩm theo ID ghi chú cha.
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<List<ProductNoteDto>> GetNotesByParentAsync(int parentId);
    }
}