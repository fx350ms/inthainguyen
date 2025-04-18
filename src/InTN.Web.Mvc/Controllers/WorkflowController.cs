using Abp.AspNetCore.Mvc.Authorization;
using InTN.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace InTN.Web.Controllers
{
    public class WorkflowController : InTNControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
