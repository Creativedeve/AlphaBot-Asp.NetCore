using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Quaestor.Bot.Configuration;
using Quaestor.Bot.EntityFrameworkCore;
using Quaestor.Bot.Migrator.DependencyInjection;
using System;

namespace Quaestor.Bot.Migrator
{
    [DependsOn(typeof(BotEntityFrameworkModule))]
    public class BotMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public BotMigratorModule(BotEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(BotMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            try
            {
                Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
               BotConsts.ConnectionStringName
           );

                Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
                Configuration.ReplaceService(
                    typeof(IEventBus),
                    () => IocManager.IocContainer.Register(
                        Component.For<IEventBus>().Instance(NullEventBus.Instance)
                    )
                );
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
                IocManager.RegisterAssemblyByConvention(typeof(BotMigratorModule).GetAssembly());
                ServiceCollectionRegistrar.Register(IocManager);
            }
            catch (Exception ex)
            {
                ;
            }

        }
    }
}
