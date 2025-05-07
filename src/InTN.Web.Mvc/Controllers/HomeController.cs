using Abp.AspNetCore.Mvc.Authorization;
using InTN.Controllers;
using InTN.IdentityCodes;
using InTN.OrderAttachments;
using InTN.OrderLogs;
using InTN.Orders;
using InTN.StatisticAndReporting;
using InTN.Web.Models.Home;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InTN.Web.Controllers;

[AbpMvcAuthorize]
public class HomeController : InTNControllerBase
{

    private readonly IStatisticReportingAppService _statisticReportingAppService;
    

    public HomeController(
        IStatisticReportingAppService statisticReportingAppService
     )
    {
        _statisticReportingAppService = statisticReportingAppService;
    }
    public async Task<IActionResult> Index()
    {
        var model = new HomePageModel();
        model.StatisticSummary = await _statisticReportingAppService.GetTotalOrdersCustomersDebtAsync();
        model.StatisticSummaryByDate = await _statisticReportingAppService.GetTotalOrdersCustomersDebtByDateAsync();
        return View(model);
    }
}
