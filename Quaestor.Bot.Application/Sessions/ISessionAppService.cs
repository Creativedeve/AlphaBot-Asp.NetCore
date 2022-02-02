using System.Threading.Tasks;
using Abp.Application.Services;
using Quaestor.Bot.Sessions.Dto;

namespace Quaestor.Bot.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
