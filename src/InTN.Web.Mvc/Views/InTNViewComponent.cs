using Abp.AspNetCore.Mvc.ViewComponents;

namespace InTN.Web.Views;

public abstract class InTNViewComponent : AbpViewComponent
{
    protected InTNViewComponent()
    {
        LocalizationSourceName = InTNConsts.LocalizationSourceName;
    }
}
