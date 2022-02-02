using Quaestor.Bot.Markets;
using System;
using System.Linq;

namespace Quaestor.Bot.EntityFrameworkCore.Seed.Tenants
{
    public class InitalMarketsBuilder
    {
        private readonly BotDbContext _context;
        public InitalMarketsBuilder(BotDbContext context)
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
                    string[] BinanceMarkets = new string[] { "ETH/BTC", "XRP/BTC", "BCHABC/BTC", "XMR/BTC", "LTC/BTC", "DASH/BTC", "STRAT/BTC", "EOS/BTC", "ETC/BTC", "DGD/BTC", "ZEC/BTC", "NXS/BTC", "BTS/BTC", "SC/BTC", "XEM/BTC", "TRX/BTC", "WAVES/BTC", "BNB/BTC", "DLT/BTC", "ADA/BTC", "ADX/BTC", "ALGO/BTC", "APPC/BTC", "ARDR/BTC", "ARN/BTC", "ATOM/BTC", "BAT/BTC", "BNT/BTC", "BQX/BTC", "BRD/BTC", "BTG/BTC", "CMT/BTC", "CVC/BTC", "DCR/BTC", "DUSK/BTC", "EDO/BTC", "ELF/BTC", "ENG/BTC", "EVX/BTC", "FET/BTC", "GAS/BTC", "GRS/BTC", "GVT/BTC", "GXS/BTC", "HC/BTC", "ICX/BTC", "INS/BTC", "IOTA/BTC", "KMD/BTC", "KNC/BTC", "LINK/BTC", "LSK/BTC", "MDA/BTC", "NANO/BTC", "NAS/BTC", "NEO/BTC", "NULS/BTC", "OMG/BTC", "ONG/BTC", "PIVX/BTC", "PPT/BTC", "RDN/BTC", "REP/BTC", "SKY/BTC", "STEEM/BTC", "WTC/BTC", "XZC/BTC", "ZEN/BTC", "ZRX/BTC", "BTC/USDT", "BNB/USDT", "ETH/USDT", "LTC/USDT", "XRP/USDT", "EOS/USDT", "TRX/USDT", "BCHABC/USDT", "LINK/USDT", "PAX/USDT", "NEO/USDT", "DUSK/USDT", "ADA/USDT", "ERD/USDT", "ETC/USDT", "XLM/USDT", "ALGO/USDT", "NANO/USDT", "ONT/USDT", "ONE/USDT", "ATOM/USDT", "QTUM/USDT", "WAVES/USDT", "XMR/USDT", "IOTA/USDT", "OMG/USDT", "DASH/USDT", "DOGE/USDT", "ZRX/USDT", "ENJ/USDT", "ONG/USDT", "NULS/USDT", "ZEC/USDT","VET/BTC" , "VET/USDT" };

                    foreach (var marketname in BinanceMarkets)
                    {
                        var isMarketExist = _context.Markets.FirstOrDefault(p => p.Name == marketname);
                        if (isMarketExist == null)
                        {
                            _context.Markets.Add(
                            new Market
                            {
                                Name = marketname,
                                ExchangeId = exchange.Id


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
