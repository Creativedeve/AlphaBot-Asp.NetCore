using Abp.Application.Services;
using Quaestor.Bot.UserSessions.Dto;
using System.Threading.Tasks;

namespace Quaestor.Bot.UserSessions
{
    public interface IUserSessionDetailAppService : IApplicationService
    {

        //Task CreateUserSessionDetail(CreateUserSessionDetailInput input);
        //Task EditUserSessionDetail(EditUserSessionDetailInput input);
        Task<UserSessionDetailDto> GetUserSessionDetailById(SearchUserSessionDetailSearch input);
        Task CreateUpdateUserSessionDetail(EditUserSessionDetailInput input);

    }
}
