using InTN.Configuration.Dto;
using System.Threading.Tasks;

namespace InTN.Configuration;

public interface IConfigurationAppService
{
    Task ChangeUiTheme(ChangeUiThemeInput input);
}
