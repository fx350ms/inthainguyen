using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.Entities;
using InTN.Orders.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.Orders
{
    public class OrderDetailAppService : AsyncCrudAppService<
        OrderDetail, // Entity chính
        OrderDetailDto, // DTO chính
        int,            // Kiểu dữ liệu của khóa chính
        PagedResultRequestDto, // DTO cho phân trang
        OrderDetailDto, // DTO cho tạo mới
        OrderDetailDto>, // DTO cho cập nhật
        IOrderDetailAppService
    {
        private readonly IRepository<OrderDetail> _repository;

        public OrderDetailAppService(IRepository<OrderDetail> repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<List<OrderDetailDto>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            var orderDetails = await _repository.GetAllListAsync(x => x.OrderId == orderId);
            return ObjectMapper.Map<List<OrderDetailDto>>(orderDetails);
        }
    }
}