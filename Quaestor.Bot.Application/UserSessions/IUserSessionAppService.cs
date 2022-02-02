using Abp.Application.Services;
using Quaestor.Bot.UserSessions.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quaestor.Bot.UserSessions
{
    public interface IUserSessionAppService : IApplicationService
    {
        UserSessionDto GetUserSessionByUserId(UserSessionSearchInput Input);
        UserSessionDto GetUserSessionByUserSessionId(UserSessionSearchInput Input);
        Task CreateUserSession(CreateUserSessionInput input);
        //Task<List<Binance.Net.Objects.BinanceOrder>> StartUserSession(UserSessionStartInput input);
        Task<string> StartUserSession(UserSessionStartInput input);

    }
}
