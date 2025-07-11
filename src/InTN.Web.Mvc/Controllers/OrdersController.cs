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
using InTN.Processes;
using InTN.ProductCategories;
using InTN.ProductTypes;
using InTN.Roles;
using InTN.Suppliers;
using InTN.Web.Models.Orders;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
        private readonly IProcessAppService _processAppService;
        private readonly IProcessStepAppService _processStepAppService;
        private readonly IRoleAppService _roleAppService;


        public OrdersController(IOrderAppService orderService,
                    IIdentityCodeAppService identityCodeAppService,
                    IOrderLogAppService orderLogAppService,
                    IOrderAttachmentAppService orderAttachmentAppService,
                    IProductTypeAppService productTypeAppService,
                    IProductCategoryAppService productCategoryAppService,
                    ISupplierAppService supplierAppService,
                    IBrandAppService brandAppService,
                    IOrderDetailAppService orderDetailAppService,
                    IFileUploadAppService fileUploadAppService,
                    IProcessAppService processAppService,
                    IProcessStepAppService processStepAppService,
                    IRoleAppService roleAppService

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
            _processAppService = processAppService;
            _processStepAppService = processStepAppService;
            _roleAppService = roleAppService;
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
                Brands = await _brandAppService.GetAllListAsync(),
                Processes = await _processAppService.GetAllListAsync()
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

            // Kiểm tra trạng thái hiện tại có phải là thiết kế không? nếu không thì không cho phép tạo thiết kế

            // Lấy ra stepInfo
            var stepInfo = await _processStepAppService.GetAsync(new EntityDto<int>(order.StepId.Value));
            if (stepInfo == null)
            {
                return NotFound();
            }


            // Lấy ra danh sách các bước tiếp theo
            var nextSteps = await _processStepAppService.GetNextStepsAsync(order.StepId.Value);
            if (nextSteps == null || !nextSteps.Any())
            {
                ModelState.AddModelError("", "Không có bước tiếp theo hợp lệ cho đơn hàng này.");
                return RedirectToAction("Detail", new { id = id });
            }

            var model = new OrderDesignUploadDto()
            {
                OrderId = order.Id,
                OrderCode = order.OrderCode,
                NextSteps = nextSteps,
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

        public async Task<IActionResult> Delivery(int id)
        {
            var order = await _orderAppService.GetAsync(new EntityDto(id));
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
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
                OrderDetails = await _orderDetailAppService.GetOrderDetailsViewByOrderIdAsync(order.Id),
                ProcessSteps = await _processStepAppService.GetNextStepsAsync(order.StepId.Value),
                ProcessName = (await _processAppService.GetAsync(new EntityDto<int>(order.ProcessId.Value))).Name
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


        public async Task<IActionResult> Process(int id, int nextStepId)
        {
            var order = await _orderAppService.GetAsync(new EntityDto(id));
            if (order == null)
            {
                return NotFound();
            }

            // Get current step by order 
            var currentStep = await _processStepAppService.GetAsync(new EntityDto<int>(order.StepId.Value));
            if (currentStep == null)
            {
                return NotFound();
            }

            // Check if the next step is valid based on the current step
            if (!currentStep.NextStepIds.Contains(nextStepId.ToString()))
            {
                ModelState.AddModelError("", "Bước tiếp theo không hợp lệ.");
                return RedirectToAction("Detail", new { id = id });
            }

            // Check if the next step is valid
            var nextStep = await _processStepAppService.GetAsync(new EntityDto<int>(nextStepId));
            if (nextStep == null)
            {
                return NotFound();
            }
            // check current user has role for next step
            if (nextStep.RoleIds != "*")
            {
                var userRoleIds = await _roleAppService.GetRoleIdsByUserIdAsync(AbpSession.UserId.Value);
                var nextStepRoleIds = nextStep.RoleIds.Split(',').Select(int.Parse).ToList();
                if (!nextStepRoleIds.Any(roleId => userRoleIds.Contains(roleId)))
                {
                    ModelState.AddModelError("", "Bạn không có quyền thực hiện bước này.");
                    return RedirectToAction("Detail", new { id = id });
                }
            }
            // Update order step

            // Kiểm tra xem người dùng muốn làm gì bước tiếp theo để thực hiện điều hướng

            switch ((OrderStatus)nextStep.OrderStatus)
            {

                // Update trạng thái 
                case OrderStatus.New:
                case OrderStatus.ReceivedRequest: // Tiếp nhận yêu cầu
                case OrderStatus.OrderConfirmed:  // Xác nhận đơn hàng
                case OrderStatus.DesignApproved:  // Đã duyệt mẫu, chuyển sang bước tiếp theo
                case OrderStatus.PrintingTest:   // Chuyển sang bước in test
                case OrderStatus.PrintingTestConfirmed: // Xác nhận in test (Ok)
                case OrderStatus.Printing:  // Chuyển sang bước in
                case OrderStatus.Processing:     // Chuyển sang bước gia công
                case OrderStatus.QcChecked:         // Đã kiểm tra QC, chuyển sang bước giao hàng
                case OrderStatus.Completed:  // Hoàn thành nghiệm thu, kết thúc quy trình
                case OrderStatus.AwaitingSampleApproval:   // Gửi duyệt mẫu
                    await _orderAppService.UpdateOrderStatusAsync(id, nextStep.Id, nextStep.OrderStatus);
                    break;

                case OrderStatus.Quoted: // Tạo phiếu báo giá
                    return RedirectToAction("CreateQuotation", new { id = id });

                case OrderStatus.Designing: // Tạo thiết kế, upload file thiết kế
                    await _orderAppService.UpdateOrderStatusAsync(id, nextStep.Id, nextStep.OrderStatus);
                    return RedirectToAction("CreateDesign", new { id = id });

                case OrderStatus.Deposited: // Chuyển sang bước đặt cọc
                    return RedirectToAction("CreateDeposit", new { id = id });

                case OrderStatus.Delivering:       // Chuyển sang bước giao hàng
                    return RedirectToAction("Delivery", new { id = id });
                default:
                    break;
            }


            return RedirectToAction("Detail", new { id = id });
        }
    }
}
