using Abp.AspNetCore.Mvc.Controllers;
using InTN.Controllers;
using InTN.Processes;
using Microsoft.AspNetCore.Mvc;

namespace InTN.Web.Mvc.Controllers
{
    public class ProcessStepGroupsController : InTNControllerBase
    {
        private readonly IProcessStepGroupAppService _processStepGroupAppService;

        public ProcessStepGroupsController(IProcessStepGroupAppService processStepGroupAppService)
        {
            _processStepGroupAppService = processStepGroupAppService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}