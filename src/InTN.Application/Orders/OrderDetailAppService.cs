using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InTN.Entities;
using InTN.FileUploads.Dto;
using InTN.Orders.Dto;
using System.Collections.Generic;
using System.Linq;
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
        public readonly IRepository<FileUpload> _fileUploadRepository;

        public OrderDetailAppService(IRepository<OrderDetail> repository, 
            IRepository<FileUpload> fileUploadRepository) : base(repository)
        {
            _fileUploadRepository = fileUploadRepository;
        }

        public async Task<List<OrderDetailDto>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            var orderDetails = await Repository.GetAllListAsync(x => x.OrderId == orderId);
             

            return ObjectMapper.Map<List<OrderDetailDto>>(orderDetails);
        }


        public async Task<List<OrderDetailViewDto>> GetOrderDetailsViewByOrderIdAsync(int orderId)
        {
            var orderDetails = await Repository.GetAllListAsync(x => x.OrderId == orderId);
            

            var fileIds = orderDetails.Select(x => x.FileId).Where(x => x.HasValue).ToList();

            var files = await _fileUploadRepository.GetAllListAsync(x => fileIds.Contains(x.Id));

            var data = new List<OrderDetailViewDto>();
            foreach (var orderDetail in orderDetails)   
            {
               var item = ObjectMapper.Map<OrderDetailViewDto>(orderDetail);

                if (item.FileId.HasValue && item.FileId > 0 && item.FileType == (int)FileType.Upload) // Chỉ lấy URL nếu FileType là Upload
                {
                    var file = files.FirstOrDefault(x => x.Id == item   .FileId.Value);
                    if (file != null)
                    {
                        item.File = ObjectMapper.Map<FileUploadDto>(file);
                    }
                }

                data.Add(item);
            }

            return data;
            
        }
    }
}