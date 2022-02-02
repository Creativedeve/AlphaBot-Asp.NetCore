using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Quaestor.Bot.Configuration;
using Quaestor.Bot.Web;

namespace Quaestor.Bot.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class BotDbContextFactory : IDesignTimeDbContextFactory<BotDbContext>
    {
        public BotDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BotDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            BotDbContextConfigurer.Configure(builder, configuration.GetConnectionString(BotConsts.ConnectionStringName));

            return new BotDbContext(builder.Options);
        }
    }
}
