using Quaestor.Bot.Exchanges.SupportedTradingMarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quaestor.Bot.EntityFrameworkCore.Seed.Tenants
{
    public class InitalSupportedCurrencies
    {
        private readonly BotDbContext _context;
        public InitalSupportedCurrencies(BotDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            try
            {
                var exchange = _context.Exchanges.FirstOrDefault(p => p.Name == "Binance");
                if (exchange != null)
                {
                    var BinanceSupportedMarkets = new Dictionary<string, decimal>
                                        {
                                            { "BTC", 0.5M },
                                            { "USDT", 0.2M }
                                        };                

                    foreach (var market in BinanceSupportedMarkets)
                    {
                        var isMarketExist = _context.SupportedTradeCurrencies.FirstOrDefault(p => p.CurrencyName == market.Key);
                        if (isMarketExist == null)
                        {
                            _context.SupportedTradeCurrencies.Add(
                            new TradingCurrencies
                            {
                                CurrencyName = market.Key,
                                MinimumValue = market.Value,
                                Exchange = exchange
                            });
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ;
            }
        }
    }
}
