using Abp.AspNetCore.Mvc.Controllers;
using InTN.Printers;
using Microsoft.AspNetCore.Mvc;

namespace InTN.Web.Mvc.Controllers
{
    public class PrintersController : AbpController
    {
        private readonly IPrinterAppService _printerAppService;

        public PrintersController(IPrinterAppService printerAppService)
        {
            _printerAppService = printerAppService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}