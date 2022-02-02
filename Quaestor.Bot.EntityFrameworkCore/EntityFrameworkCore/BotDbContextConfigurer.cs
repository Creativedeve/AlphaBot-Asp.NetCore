using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Quaestor.Bot.EntityFrameworkCore
{
    public static class BotDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<BotDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<BotDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
