using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Quaestor.Bot.Authorization.Roles;
using Quaestor.Bot.Authorization.Users;
using Quaestor.Bot.MultiTenancy;
using Quaestor.Bot.Exchanges;
using Quaestor.Bot.UserKeys;
using Quaestor.Bot.Markets;
using Quaestor.Bot.Tickers;
using Quaestor.Bot.UserSessions;
using Quaestor.Bot.PurchaseOrderRecords;
using Quaestor.Bot.TradeProfitRates;
using Quaestor.Bot.ErrorLogs;
using Quaestor.Bot.TradingRules;
using Quaestor.Bot.PreserveBuyInformation;
using Quaestor.Bot.Exchanges.SupportedTradingMarket;
using Quaestor.Bot.Products;

namespace Quaestor.Bot.EntityFrameworkCore
{
    public class BotDbContext : AbpZeroDbContext<Tenant, Role, User, BotDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Exchange> Exchanges { get; set; }
        public DbSet<UserKey> UserKeys { get; set; }
        public DbSet<Market> Markets { get; set; }
        public DbSet<Ticker> Ticker { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<UserSessionDetail> UserSessionDetail { get; set; }
        public DbSet<PurchaseOrderRecord> PurchaseOrderRecords { get; set; }
        public DbSet<PurchaseOrderRecordDetail> PurchaseOrderRecordDetail { get; set; }
        public DbSet<TradeProfitRate> TradeProfitRate { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }

        public DbSet<TradingParameter> TradingParameters { get; set; }

        public DbSet<PreserveBuyInfo> PreserveBuyInfo { get; set; }

        public DbSet<TradeProfitRateDetail> TradeProfitRateDetail { get; set; }

        public DbSet<TradeProfitSellInfo> TradeProfitSellInfo { get; set; }

        public DbSet<TradingCurrencies> SupportedTradeCurrencies { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<UserProducts> UserProducts { get; set; }
        public DbSet<UserProductsPaymentRecords> UserProductsPaymentRecords { get; set; }
        public BotDbContext(DbContextOptions<BotDbContext> options)
            : base(options)
        {
        }
    }
}
