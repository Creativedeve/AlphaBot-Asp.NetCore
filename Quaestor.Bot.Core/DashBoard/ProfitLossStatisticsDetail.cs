using System;
using System.Collections.Generic;
using System.Text;

namespace Quaestor.Bot.DashBoard
{
    public class ProfitLossStatisticsDetail
    {
        public string MarketName { get; set; }
        public int MarketId { get; set; }        
        public decimal TotalBoughtQuantity { get; set; }
        public decimal AverageValue { get; set; }
        public int TotalBuy { get; set; }
        public int UserSessionId { get; set; }
        public bool IsSessionClosed { get; set; }
        public int UserId { get; set; }
        public decimal BTCAllocated { get; set; }
        public decimal FirstBuyEquityPercentage { get; set; }
        public decimal TradeProfitPercentage { get; set; }
        public decimal TradeProfitSaleRate { get; set; }
        public int PurchaseOrderRecordId { get; set; }
        public decimal SellAmount { get; set; }
        public decimal SellQuantity { get; set; }
        public decimal SellRate { get; set; }
        public decimal BTCValueAfterTP { get; set; }
        public decimal TradePercentage { get; set; }
        public decimal BTCTradeProfit { get; set; }
        public decimal TotalBTCAfterSell { get; set; }
        public decimal MarketPercentage { get; set; }
        public bool IsSold { get; set; }
        public bool IsAlreadySold { get; set; }
        public decimal SumBuyWith { get; set; }

    }
}
