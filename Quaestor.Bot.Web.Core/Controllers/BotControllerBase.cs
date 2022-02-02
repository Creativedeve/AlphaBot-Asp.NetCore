using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Quaestor.Bot.Controllers
{
    public abstract class BotControllerBase: AbpController
    {
        protected BotControllerBase()
        {
            LocalizationSourceName = BotConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
