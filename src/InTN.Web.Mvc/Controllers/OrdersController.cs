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
            // _orderAppService.UpdateStatusToDepositedAsync
            switch ((OrderStatus)nextStep.OrderStatus)
            {
                case OrderStatus.New:
                case OrderStatus.ReceivedRequest:
                    // Update trạng thái 
                    break;
                case OrderStatus.Quoted:
                    return RedirectToAction("CreateQuotation", new { id = id });
                    break;
                case OrderStatus.OrderConfirmed:
                   // Update trạng thái xác nhận đơn hàng
                    break;
                case OrderStatus.Designing:
                    return RedirectToAction("CreateDesign", new { id = id });
                    break;
                case OrderStatus.AwaitingSampleApproval:
                    break;
                case OrderStatus.DesignApproved:
                    break;
                case OrderStatus.Deposited:
                    return RedirectToAction("CreateDeposit", new { id = id });
                    break;
                case OrderStatus.PrintingTest:
                    break;
                case OrderStatus.PrintingTestConfirmed:
                    break;
                case OrderStatus.Printing:
                    break;
                case OrderStatus.Processing:
                    break;
                case OrderStatus.QcChecked:
                    break;
                case OrderStatus.Delivering:
                    break;
                case OrderStatus.Completed:
                    break;
                default:
                    break;
            }


            return RedirectToAction("Detail", new { id = id });
        }
    }
}
