using Abp.AutoMapper;
using Abp.Hangfire;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Hangfire;
using Quaestor.Bot.Authorization;
using Quaestor.Bot.JobManagement;
using System;

namespace Quaestor.Bot
{
    [DependsOn(
        typeof(BotCoreModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpHangfireAspNetCoreModule),
        typeof(BotJobManagmentModule)
        )]
    public class BotApplicationModule : AbpModule
    {
        //private readonly IHostingEnvironment _env;
        //private readonly IConfigurationRoot _appConfiguration;

        //private  IQuaestorBotSyncBus<RebuyDetail> quaestorBotSyncBus;

        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<BotAuthorizationProvider>();
            Configuration.BackgroundJobs.IsJobExecutionEnabled = true;
            //Configuration.Caching.UseRedis(options =>
            //{
            //    var connectionString = _appConfiguration["Abp:RedisCache:ConnectionString"];
            //    if (connectionString != null && connectionString != "localhost")
            //    {
            //        options.ConnectionString = AsyncHelper.RunSync(() => Dns.GetHostAddressesAsync(connectionString))[0].ToString();
            //    }
            //});
        }
        public override void Initialize()
        {
            try
            {
                var thisAssembly = typeof(BotApplicationModule).GetAssembly();

                IocManager.RegisterAssemblyByConvention(thisAssembly);
                Configuration.Modules.AbpAutoMapper().Configurators.Add(
                    // Scan the assembly for classes which inherit from AutoMapper.Profile
                    cfg => cfg.AddProfiles(thisAssembly)
                );
            }
            catch (Exception ex)
            {
                ;
            }

        }
        public override void PostInitialize()
        {
            //quaestorBotSyncBus = new QuaestorBotSyncBus<RebuyDetail>();
            //quaestorBotSyncBus.RebuyQueueSubscriber();
            //quaestorBotSyncBus.FirstbuyQueueSubscriber();

            //RecurringJob.RemoveIfExists("JobEnQueueProcess");
            //RecurringJob.AddOrUpdate("JobEnQueueProcess", () => JobEnQueueProcess(), Cron.MinuteInterval(5));

            // RecurringJob.RemoveIfExists("FetchCalculatedRebuyInfo");
            //    RecurringJob.AddOrUpdate("FetchCalculatedRebuyInfo", () => EnQueueActiveUserSessionInCasheJob(), Cron.Minutely());
        }
        //public void JobEnQueueProcess()
        //{
        //    //IocManager.Resolve<TickerDataManagement>().Execute();
        //    //IocManager.Resolve<FetchCalculatedRebuyInfo>().Execute();
        //    //IocManager.Resolve<FirstBuyManagement>().Execute();
        //    //IocManager.Resolve<BinancePlaceOrder>().Execute();

        //}

    }
}
