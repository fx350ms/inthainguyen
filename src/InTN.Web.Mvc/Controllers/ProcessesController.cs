using Abp.Application.Services.Dto;
using InTN.Controllers;
using InTN.Processes;
using InTN.Web.Models.Processes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InTN.Web.Controllers
{
    public class ProcessesController : InTNControllerBase
    {
        private readonly IProcessAppService _processAppService;
        private readonly IProcessStepAppService _processStepAppService;
        private readonly IProcessStepGroupAppService _processStepGroupAppService;

        public ProcessesController(IProcessAppService processAppService,
            IProcessStepAppService processStepAppService,
            IProcessStepGroupAppService processStepGroupAppService
            )
        {
            _processAppService = processAppService;
            _processStepAppService = processStepAppService;
            _processStepGroupAppService = processStepGroupAppService;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Details(int id)
        {
            // Here you would typically retrieve the process details by id
            // For now, we will just return a view with the id
            ViewBag.ProcessId = id;
            return View();
        }

        public async Task<IActionResult> Config(int id)
        {
            var process = await _processAppService.GetAsync(new EntityDto<int>(id));

            if (process == null)
            {
                return NotFound();
            }
            ViewBag.ProcessId = id;

            var model = new ProcessConfigViewModel
            {
                Process = process,
                ProcessStepGroups = await _processStepGroupAppService.GetAllListAsync(),
                //ProcessSteps = await _processStepAppService.GetAllListAsync()
            };

            return View(model);

        }
    }
}
