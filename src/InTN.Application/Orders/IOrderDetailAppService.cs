using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InTN.Orders.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.Orders
{
    public interface IOrderDetailAppService : IAsyncCrudAppService<
        OrderDetailDto, // DTO chính
        int,            // Kiểu dữ liệu của khóa chính
        PagedResultRequestDto, // DTO cho phân trang
        OrderDetailDto, // DTO cho tạo mới
        OrderDetailDto> // DTO cho cập nhật
    {
        Task<List<OrderDetailDto>> GetOrderDetailsByOrderIdAsync(int orderId);
        Task<List<OrderDetailViewDto>> GetOrderDetailsViewByOrderIdAsync(int orderId);
    }
}