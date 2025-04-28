using InTN.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InTN.Web.Controllers
{
    public class OrdersController : InTNControllerBase
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
    }
}
