using System;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Zero.Configuration;
using Quaestor.Bot.Authorization.Accounts.Dto;
using Quaestor.Bot.Authorization.Users;

namespace Quaestor.Bot.Authorization.Accounts
{
    public class AccountAppService : BotAppServiceBase, IAccountAppService
    {
        private readonly UserRegistrationManager _userRegistrationManager;

        public AccountAppService(
            UserRegistrationManager userRegistrationManager)
        {
            _userRegistrationManager = userRegistrationManager;
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            try
            {
                var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);

                if (tenant == null)
                {
                    return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
                }

                if (!tenant.IsActive)
                {
                    return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
                }

                return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
            }
            catch(Exception ex)
            {
                return null;
            }            
        }

        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            var user = await _userRegistrationManager.RegisterAsync(
                input.Name,
                input.Surname,
                input.EmailAddress,
                input.UserName,
                input.Password,
                true // Assumed email address is always confirmed. Change this if you want to implement email confirmation.
            );

            var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

            return new RegisterOutput
            {
                CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
            };
        }
    }
}
