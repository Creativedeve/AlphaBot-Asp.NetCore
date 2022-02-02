using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using Quaestor.Bot.Authorization.Roles;
using Quaestor.Bot.Authorization.Users;
using Quaestor.Bot.Configuration;
using Quaestor.Bot.Localization;
using Quaestor.Bot.MultiTenancy;
using Quaestor.Bot.Timing;
using System;

namespace Quaestor.Bot
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class BotCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            try
            {
                Configuration.Auditing.IsEnabledForAnonymousUsers = true;

                // Declare entity types
                Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
                Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
                Configuration.Modules.Zero().EntityTypes.User = typeof(User);

                BotLocalizationConfigurer.Configure(Configuration.Localization);

                // Enable this line to create a multi-tenant application.
                Configuration.MultiTenancy.IsEnabled = BotConsts.MultiTenancyEnabled;

                // Configure roles
                AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

                Configuration.Settings.Providers.Add<AppSettingProvider>();
            }
            catch (Exception ex)
            {
                ;
            }

        }

        public override void Initialize()
        {
            try
            {
                IocManager.RegisterAssemblyByConvention(typeof(BotCoreModule).GetAssembly());
            }
            catch (Exception ex)
            {
                ;
            }
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
