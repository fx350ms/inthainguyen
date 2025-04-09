using Abp.Authorization;
using Abp.Runtime.Session;
using InTN.Configuration.Dto;
using System.Threading.Tasks;

namespace InTN.Configuration;

[AbpAuthorize]
public class ConfigurationAppService : InTNAppServiceBase, IConfigurationAppService
{
    public async Task ChangeUiTheme(ChangeUiThemeInput input)
    {
        await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
    }
}
