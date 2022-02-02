using System;
using System.Collections.Generic;
using System.Text;

namespace Quaestor.Bot.DashBoard
{
    public class DashboardResult
    {
        public string MarketName { get; set; }
        public int MarketId { get; set; }
        public decimal Quantity { get; set; }
        public decimal AverageValue { get; set; }
        public decimal NextBuyPoint { get; set; }
        public int TotalBuy { get; set; }
        public decimal TradeProfitSaleRate { get; set; }
        public int UserSessionId { get; set; }
        public bool IsSessionClosed { get; set; }

        public decimal TickerValue { get; set; }
    }
}
