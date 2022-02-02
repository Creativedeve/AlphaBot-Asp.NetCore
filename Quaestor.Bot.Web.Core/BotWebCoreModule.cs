using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using Quaestor.Bot.Authentication.JwtBearer;
using Quaestor.Bot.Configuration;
using Quaestor.Bot.EntityFrameworkCore;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Hangfire;
using Quaestor.Bot.JobManagement.StateHandler;
using Quaestor.Bot.JobManagement;
using Quaestor.Bot.JobManagement.Jobs.JobImplementation;

namespace Quaestor.Bot
{
    [DependsOn(
         typeof(BotApplicationModule),
         typeof(BotEntityFrameworkModule),
         typeof(AbpAspNetCoreModule)
        , typeof(AbpAspNetCoreSignalRModule),
        typeof(AbpHangfireAspNetCoreModule),
        typeof(BotJobManagmentModule)
     )]
    public class BotWebCoreModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;
        private IRebuySynchronization quaestorBotRebuySyncBus;
        private IFirstBuySynchronization quaestorBotFirstBuySyncBus;
        private IRetrySynchronization quaestorBotRetrySyncBus;
        private ITradeProfitSynchronization quaestorBotTradeProfitSyncBus;
        private IKeyUpDownSynchronization quaestorBotKeyUpDownSyncBus;
        private ISaleImmediatelySynchronization quaestorBotSaleImmediately;
        private static readonly object _objKey = new object();
        private static readonly object _objTrade = new object();
        private static readonly object _objFirstBuy = new object();
        private static readonly object _objRebuy = new object();
        private bool isAllMarketsTickerExist = false;

        public BotWebCoreModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                BotConsts.ConnectionStringName
            );
            Configuration.BackgroundJobs.UseHangfire();


            GlobalConfiguration.Configuration.UseSqlServerStorage(_appConfiguration.GetConnectionString(
                BotConsts.ConnectionStringName
            ));
            var storage = JobStorage.Current;
            //no exception
            storage = JobStorage.Current;
            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(BotApplicationModule).GetAssembly()
                 );

            ConfigureTokenAuth();
        }
        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BotWebCoreModule).GetAssembly());
        }
        public void CacheMonitorJobProcess()
        {
            IocManager.Resolve<CacheMonitoringManagement>().Execute();
        }
        public void EmailJobPackageExpiration()
        {
            IocManager.Resolve<UserPackageExpiryManagement>().Execute();
        }

        #region JobProcess
        public override void PostInitialize()
        {

            IocManager.Resolve<BinanceCandlestickDataManagement>().Execute();

            quaestorBotRebuySyncBus = new RebuySynchronization();
            quaestorBotFirstBuySyncBus = new FirstBuySynchronization();
            quaestorBotRetrySyncBus = new RetrySynchronization();
            quaestorBotTradeProfitSyncBus = new TradeProfitSynchronization();
            quaestorBotKeyUpDownSyncBus = new KeyUpDownSynchronization();
            quaestorBotSaleImmediately = new SaleImmediatelySynchronization();
            quaestorBotFirstBuySyncBus.FirstbuyQueueSubscriber();
            quaestorBotRebuySyncBus.RebuyQueueSubscriber();
            quaestorBotRetrySyncBus.RetryQueueSubscriber();
            quaestorBotTradeProfitSyncBus.TradeProfitQueueSubscriber();
            quaestorBotKeyUpDownSyncBus.KeyUpDownQueueSubscriber();
            quaestorBotSaleImmediately.SaleImmediatelyQueueSubscriber();

            //var cronexp = "*/40 * * * * *";
            RecurringJob.RemoveIfExists("TradeProfitManagement");
            RecurringJob.AddOrUpdate("TradeProfitManagement", () => TradeProfitJobProcess(), Cron.Minutely);

            RecurringJob.RemoveIfExists("FirstBuyManagement");
            RecurringJob.AddOrUpdate("FirstBuyManagement", () => FirstBuyJobProcess(), Cron.Minutely);

            RecurringJob.RemoveIfExists("RebuyManagement");
            RecurringJob.AddOrUpdate("RebuyManagement", () => RebuyJobProcess(), Cron.Minutely);

            RecurringJob.RemoveIfExists("KeyUpDownJobProcess");
            RecurringJob.AddOrUpdate("KeyUpDownJobProcess", () => KeyUpDownJobProcess(), Cron.Minutely);

            var cronexpcache = "*/30 * * * * *";
            RecurringJob.RemoveIfExists("CacheMonitorJob");
            RecurringJob.AddOrUpdate("CacheMonitorJob", () => CacheMonitorJobProcess(), cronexpcache);

            // var cronDailymidnight = "0 0 * * *";
            RecurringJob.RemoveIfExists("EmailJobPackageExpiration");
            RecurringJob.AddOrUpdate("EmailJobPackageExpiration", () => EmailJobPackageExpiration(), Cron.Daily);

        }

        
        public void TradeProfitJobProcess()
        {
            if(IsAllMarketTickerExistsInCache())
            {
                lock (_objTrade)
                {
                    IocManager.Resolve<TradeProfitManagement>().Execute();
                }
            }
        }
        public void FirstBuyJobProcess()
        {
            
            if(IsAllMarketTickerExistsInCache())
            {
                lock (_objFirstBuy)
                {
                    IocManager.Resolve<FirstBuyManagement>().Execute();
                }
            }
        }
        public void RebuyJobProcess()
        {
            if(IsAllMarketTickerExistsInCache())
            {
                lock (_objRebuy)
                {
                    IocManager.Resolve<RebuyManagement>().Execute();
                }
            }
        }
        public void KeyUpDownJobProcess()
        {
            if(IsAllMarketTickerExistsInCache())
            {
                lock (_objKey)
                {
                    IocManager.Resolve<KeyUpDownManagement>().Execute();
                }
            }
        }
        public bool IsAllMarketTickerExistsInCache()
        {
            if (!isAllMarketsTickerExist)
            {
                isAllMarketsTickerExist = IocManager.Resolve<BinanceCandlestickDataManagement>().ExecuteUserOrder(0).Result;
            }
            return isAllMarketsTickerExist;
        }
        #endregion
    }
}
