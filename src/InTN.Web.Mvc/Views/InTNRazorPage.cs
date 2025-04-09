using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace InTN.Web.Views;

public abstract class InTNRazorPage<TModel> : AbpRazorPage<TModel>
{
    [RazorInject]
    public IAbpSession AbpSession { get; set; }

    protected InTNRazorPage()
    {
        LocalizationSourceName = InTNConsts.LocalizationSourceName;
    }
}
