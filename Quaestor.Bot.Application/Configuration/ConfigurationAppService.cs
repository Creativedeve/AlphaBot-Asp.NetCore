using System;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Quaestor.Bot.Configuration.Dto;

namespace Quaestor.Bot.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : BotAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            try
            {
                await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
            }
            catch (Exception ex)
            {
                await Task.FromResult(0);
            }
        }
    }
}
