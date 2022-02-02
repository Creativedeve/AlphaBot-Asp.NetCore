using System.Threading.Tasks;
using Abp.Application.Services;
using Quaestor.Bot.Authorization.Accounts.Dto;

namespace Quaestor.Bot.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
