using System.Threading.Tasks;
using Quaestor.Bot.Configuration.Dto;

namespace Quaestor.Bot.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
