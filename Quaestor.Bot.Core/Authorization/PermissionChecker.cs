using Abp.Authorization;
using Quaestor.Bot.Authorization.Roles;
using Quaestor.Bot.Authorization.Users;

namespace Quaestor.Bot.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
