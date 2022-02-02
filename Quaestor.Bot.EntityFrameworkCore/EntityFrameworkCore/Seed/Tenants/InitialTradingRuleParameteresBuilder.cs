using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Quaestor.Bot.TradingRules;


namespace Quaestor.Bot.EntityFrameworkCore.Seed.Tenants
{
    class InitialTradingRuleParameteresBuilder
    {
        private readonly BotDbContext _context;
        public InitialTradingRuleParameteresBuilder(BotDbContext context)
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
                    var eth = _context.TradingParameters.FirstOrDefault(p => p.Pair == "ETH/BTC");
                    if (eth == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "ETH/BTC",
                                MinTradeAmount = Convert.ToDecimal(0.001),
                                MarketName = "ETH",
                                MinTickSize = Convert.ToDecimal(0.000001),
                                MinOrderValue = Convert.ToDecimal(0.001)

                            });
                    }
                    var XRP = _context.TradingParameters.FirstOrDefault(p => p.Pair == "XRP/BTC");
                    if (XRP == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "XRP/BTC",
                                MinTradeAmount = 1,
                                MarketName = "XRP",
                                MinTickSize = Convert.ToDecimal(0.00000001),
                                MinOrderValue = Convert.ToDecimal(0.001)

                            });
                    }
                    var BCHABC = _context.TradingParameters.FirstOrDefault(p => p.Pair == "BCHABC/BTC");
                    if (BCHABC == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "BCHABC/BTC",
                                MinTradeAmount = Convert.ToDecimal(0.001),
                                MarketName = "BCHABC",
                                MinTickSize = Convert.ToDecimal(0.000001),
                                MinOrderValue = Convert.ToDecimal(0.001)

                            });
                    }
                    var BCHSV = _context.TradingParameters.FirstOrDefault(p => p.Pair == "BCHSV/BTC");
                    if (BCHSV == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "BCHSV/BTC",
                                MinTradeAmount = Convert.ToDecimal(0.001),
                                MarketName = "BCHSV",
                                MinTickSize = Convert.ToDecimal(0.000001),
                                MinOrderValue = Convert.ToDecimal(0.001)
                            });
                    }
                    var XMR = _context.TradingParameters.FirstOrDefault(p => p.Pair == "XMR/BTC");
                    if (XMR == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "XMR/BTC",
                                MinTradeAmount = Convert.ToDecimal(0.001),
                                MarketName = "XMR",
                                MinTickSize = Convert.ToDecimal(0.000001),
                                MinOrderValue = Convert.ToDecimal(0.001)
                            
                            });
                    }
                    var LTC = _context.TradingParameters.FirstOrDefault(p => p.Pair == "LTC/BTC");
                    if (LTC == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "LTC/BTC",
                                MinTradeAmount = Convert.ToDecimal(0.01),
                                MarketName = "LTC",
                                MinTickSize = Convert.ToDecimal(0.000001),
                                MinOrderValue = Convert.ToDecimal(0.001)
                            });
                    }
                    var DASH = _context.TradingParameters.FirstOrDefault(p => p.Pair == "DASH/BTC");
                    if (DASH == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "DASH/BTC",
                                MinTradeAmount = Convert.ToDecimal(0.001),
                                MarketName = "DASH",
                                MinTickSize = Convert.ToDecimal(0.000001),
                                MinOrderValue = Convert.ToDecimal(0.001)
                            });
                    }
                    var STRAT = _context.TradingParameters.FirstOrDefault(p => p.Pair == "STRAT/BTC");
                    if (STRAT == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "STRAT/BTC",
                                MinTradeAmount = Convert.ToDecimal(0.01),
                                MarketName = "STRAT",
                                MinTickSize = Convert.ToDecimal(0.000001),
                                MinOrderValue = Convert.ToDecimal(0.001)
                            });
                    }
                    var EOS = _context.TradingParameters.FirstOrDefault(p => p.Pair == "EOS/BTC");
                    if (EOS == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "EOS/BTC",
                                MinTradeAmount = 1,
                                MarketName = "EOS",
                                MinTickSize = Convert.ToDecimal(0.00000001),
                                MinOrderValue = Convert.ToDecimal(0.001)
                            });
                    }
                    var ETC = _context.TradingParameters.FirstOrDefault(p => p.Pair == "ETC/BTC");
                    if (ETC == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "ETC/BTC",
                                MinTradeAmount = Convert.ToDecimal(0.01),
                                MarketName = "ETC",
                                MinTickSize = Convert.ToDecimal(0.000001),
                                MinOrderValue = Convert.ToDecimal(0.001)
                            });
                    }
                    var DGD = _context.TradingParameters.FirstOrDefault(p => p.Pair == "DGD/BTC");
                    if (DGD == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "DGD/BTC",
                                MinTradeAmount = Convert.ToDecimal(0.001),
                                MarketName = "DGD",
                                MinTickSize = Convert.ToDecimal(0.000001),
                                MinOrderValue = Convert.ToDecimal(0.001 )                          
                            });
                    }
                    var ZEC = _context.TradingParameters.FirstOrDefault(p => p.Pair == "ZEC/BTC");
                    if (ZEC == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "ZEC/BTC",
                                MinTradeAmount = Convert.ToDecimal(0.001),
                                MarketName = "ZEC",
                                MinTickSize = Convert.ToDecimal(0.000001),
                                MinOrderValue = Convert.ToDecimal(0.001)
                            });
                    }
                    var NXS = _context.TradingParameters.FirstOrDefault(p => p.Pair == "NXS/BTC");
                    if (NXS == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "NXS/BTC",
                                MinTradeAmount = Convert.ToDecimal(0.01),
                                MarketName = "NXS",
                                MinTickSize = Convert.ToDecimal(0.0000001),
                                MinOrderValue = Convert.ToDecimal(0.001)
                            });
                    }
                    var BTS = _context.TradingParameters.FirstOrDefault(p => p.Pair == "BTS/BTC");
                    if (BTS == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "BTS/BTC",
                                MinTradeAmount = 1,
                                MarketName = "BTS",
                                MinTickSize = Convert.ToDecimal(0.00000001),
                                MinOrderValue = Convert.ToDecimal(0.001)
                            });
                    }
                    var SC = _context.TradingParameters.FirstOrDefault(p => p.Pair == "SC/BTC");
                    if (SC == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "SC/BTC",
                                MinTradeAmount = 1,
                                MarketName = "SC",
                                MinTickSize = Convert.ToDecimal(0.00000001),
                                MinOrderValue = Convert.ToDecimal(0.001)
                            });
                    }
                    var XEM = _context.TradingParameters.FirstOrDefault(p => p.Pair == "XEM/BTC");
                    if (XEM == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "XEM/BTC",
                                MinTradeAmount = 1,
                                MarketName = "XEM",
                                MinTickSize = Convert.ToDecimal(0.00000001),
                                MinOrderValue = Convert.ToDecimal(0.001)
                            });
                    }
                    var TRX = _context.TradingParameters.FirstOrDefault(p => p.Pair == "TRX/BTC");
                    if (TRX == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "TRX/BTC",
                                MinTradeAmount = 1,
                                MarketName = "TRX",
                                MinTickSize = Convert.ToDecimal(0.00000001),
                                MinOrderValue = Convert.ToDecimal(0.001)
                            });
                    }
                    var WAVES = _context.TradingParameters.FirstOrDefault(p => p.Pair == "WAVES/BTC");
                    if (WAVES == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "WAVES/BTC",
                                MinTradeAmount = Convert.ToDecimal(0.01),
                                MarketName = "WAVES",
                                MinTickSize = Convert.ToDecimal(0.000001),
                                MinOrderValue = Convert.ToDecimal(0.001)
                            });
                    }
                    var BNB = _context.TradingParameters.FirstOrDefault(p => p.Pair == "BNB/BTC");
                    if (BNB == null)
                    {
                        _context.TradingParameters.Add(
                            new TradingParameter
                            {
                                ExchangeId = exchange.Id,
                                Pair = "BNB/BTC",
                                MinTradeAmount = 1,
                                MarketName = "BNB",
                                MinTickSize = Convert.ToDecimal(0.00000001),
                                MinOrderValue = Convert.ToDecimal(0.001)                            
                            });
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
