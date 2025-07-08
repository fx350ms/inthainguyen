using Abp.Application.Services.Dto;
using InTN.Brands;
using InTN.Controllers;
using InTN.Customers;
using InTN.FileUploads;
using InTN.IdentityCodes;
using InTN.OrderAttachments;
using InTN.OrderLogs;
using InTN.Orders;
using InTN.Orders.Dto;
using InTN.ProductCategories;
using InTN.ProductTypes;
using InTN.Suppliers;
using InTN.Web.Models.Orders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InTN.Web.Controllers
{
    public class OrdersController : InTNControllerBase
    {
        private readonly IIdentityCodeAppService _identityCodeAppService;
        private readonly IOrderAppService _orderAppService;
        private readonly IOrderLogAppService _orderLogAppService;
        private readonly IOrderAttachmentAppService _orderAttachmentAppService;
        private readonly IProductTypeAppService _productTypeAppService;

        private readonly IProductCategoryAppService _productCategoryAppService;
        private readonly ISupplierAppService _supplierAppService;
        private readonly IBrandAppService _brandAppService;
        private readonly IOrderDetailAppService _orderDetailAppService;
        private readonly IFileUploadAppService _fileAppService;


        public OrdersController(IOrderAppService orderService,
                    IIdentityCodeAppService identityCodeAppService,
                    IOrderLogAppService orderLogAppService,
                    IOrderAttachmentAppService orderAttachmentAppService,
                    IProductTypeAppService productTypeAppService,
                    IProductCategoryAppService productCategoryAppService,
                    ISupplierAppService supplierAppService,
                    IBrandAppService brandAppService,
                    IOrderDetailAppService orderDetailAppService,
                    IFileUploadAppService fileUploadAppService

         )
        {
            _orderAppService = orderService;
            _identityCodeAppService = identityCodeAppService;
            _orderLogAppService = orderLogAppService;
            _orderAttachmentAppService = orderAttachmentAppService;
            _productTypeAppService = productTypeAppService;
            _productCategoryAppService = productCategoryAppService;
            _supplierAppService = supplierAppService;
            _brandAppService = brandAppService;
            _orderDetailAppService = orderDetailAppService;
            _fileAppService = fileUploadAppService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var identityCode = await _identityCodeAppService.GenerateNewSequentialNumberAsync("DH");
            var model = new CreateOrderModel
            {
                CreateOrderDto = new CreateOrderDto()
                { OrderCode = identityCode.Code },
                ProductTypes = await _productTypeAppService.GetAllListAsync(),
                ProductCategories = await _productCategoryAppService.GetAllListAsync(),
                Suppliers = await _supplierAppService.GetAllListAsync(),
                Brands = await _brandAppService.GetAllListAsync()
            };

            return View(model);
        }


        public async Task<IActionResult> CreateItemDetail()
        {
            return PartialView("_Create.ProductItem");
        }



        public async Task<IActionResult> CreateQuotation(int id)
        {
            var order = await _orderAppService.GetAsync(new EntityDto(id));
            if (order == null)
            {
                return NotFound();
            }
            var model = new OrderQuotationUploadDto()
            {
                OrderId = order.Id,
                OrderCode = order.OrderCode,

            };
            return View(model);
        }



        public async Task<IActionResult> CreateDesign(int id)
        {
            var order = await _orderAppService.GetAsync(new EntityDto(id));
            if (order == null)
            {
                return NotFound();
            }
            var model = new OrderDesignUploadDto()
            {
                OrderId = order.Id,
                OrderCode = order.OrderCode,
            };
            return View(model);
        }


        public async Task<IActionResult> CreateDeposit(int id)
        {
            var order = await _orderAppService.GetAsync(new EntityDto(id));
            if (order == null)
            {
                return NotFound();
            }
            var model = new OrderDepositUploadDto()
            {
                OrderId = order.Id,
                OrderCode = order.OrderCode,
                TotalAmount = order.TotalAmount ?? 0
            };
            return View(model);
        }


        public async Task<IActionResult> Payment(int id)
        {
            var order = await _orderAppService.GetAsync(new EntityDto(id));
            if (order == null)
            {
                return NotFound();
            }

            var model = new OrderTransactionUploadDto()
            {
                OrderId = order.Id,
                OrderCode = order.OrderCode,
                TotalAmount = order.TotalAmount ?? 0,
                TotalDeposit = order.TotalDeposit ?? 0,
                PaymentStatus = order.PaymentStatus
            };
            return View(model);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var order = await _orderAppService.GetAsync(new EntityDto(id));
            if (order == null)
            {
                return NotFound();
            }
            var model = new OrderDetailModel()
            {
                OrderDto = order,
                OrderLogs = await _orderLogAppService.GetOrderLogsByOrderIdAsync(order.Id),
              //  OrderAttachments = await _orderAttachmentAppService.GetAttachmentsByOrderIdAsync(order.Id),
                OrderDetails = await _orderDetailAppService.GetOrderDetailsViewByOrderIdAsync(order.Id)
            };
            return View(model);
        }


        public async Task<IActionResult> DownloadAttachment(int fileId, string fileName)
        {
            var file = await _fileAppService.GetAsync(new EntityDto<int>(fileId));
            if (file == null || file.FileContent == null)
            {
                return NotFound();
            }

            return File(file.FileContent, "application/octet-stream", fileName);
        }

    }
}
