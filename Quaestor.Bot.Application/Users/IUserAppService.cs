using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Quaestor.Bot.Roles.Dto;
using Quaestor.Bot.Users.Dto;

namespace Quaestor.Bot.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);
		Task<Google2FA> UpdateIs2FaEnable(string userId);

        //tanseef
        Task<UserDto> GetUserByEmailAsync(string Email);

    }
}
