using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Auditing;
using Quaestor.Bot.Sessions.Dto;

namespace Quaestor.Bot.Sessions
{
    public class SessionAppService : BotAppServiceBase, ISessionAppService
    {
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            try
            {
                var output = new GetCurrentLoginInformationsOutput
                {
                    Application = new ApplicationInfoDto
                    {
                        Version = AppVersionHelper.Version,
                        ReleaseDate = AppVersionHelper.ReleaseDate,
                        Features = new Dictionary<string, bool>()
                    }
                };

                if (AbpSession.TenantId.HasValue)
                {
                    output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
                }

                if (AbpSession.UserId.HasValue)
                {
                    output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
                }

                return output;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
