using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Quaestor.Bot.UserKeys.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quaestor.Bot.UserKeys
{
    public interface IUserKeyAppService : IApplicationService
    {

        ListResultDto<UserKeyListDto> GetUserKeyByUserId(UserKeySearchByUserIdInput input);
        UserKeyDto GetUserKeyById(UserKeySearchInput input);
        Task<BalanceInfo> CreateUserKey(CreateUserKeyInput input);
        Task<BalanceInfo> EditUserKey(EditUserKeyInput input);       
    }
}
