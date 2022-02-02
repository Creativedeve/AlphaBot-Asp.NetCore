using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Quaestor.Bot.Configuration;
using Binance.Net.Objects;

namespace Quaestor.Bot.Web.Host.Startup
{
    [DependsOn(
       typeof(BotWebCoreModule))]
    public class BotWebHostModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public BotWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BotWebHostModule).GetAssembly());
        }
       
    }
}
