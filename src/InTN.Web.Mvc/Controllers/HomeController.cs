using Abp.AspNetCore.Mvc.Authorization;
using InTN.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace InTN.Web.Controllers;

[AbpMvcAuthorize]
public class HomeController : InTNControllerBase
{
    public ActionResult Index()
    {
        return View();
    }
}
