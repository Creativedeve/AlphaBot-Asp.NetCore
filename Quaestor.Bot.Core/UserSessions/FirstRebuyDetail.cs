namespace Quaestor.Bot.UserSessions
{
    public class RebuyDetail
    {
        public string MarketName { get; set; }
        public int MarketId { get; set; }
        public int PurchaseOrderRecordId { get; set; }
        public int ReBuySequence { get; set; }
        public decimal RebuyRate { get; set; }
        public decimal ReBuyWith { get; set; }
        public int UserId { get; set; }
        public int UserSessionDetailId { get; set; }

    }
    public class FirstBuyDetail
    {

        public int UserId { get; set; }
        public string MarketName { get; set; }
        public decimal FirstBuyWith { get; set; }
        public int UserSessionDetailId { get; set; }
        public int MarketId { get; set; }
        public decimal? BuyStopDownRate { get; set; }
        public decimal? BuyStopUpRate { get; set; }
        public decimal? KeyUpDownRate { get; set; }
    }

    public class RetryDetail : FirstBuyDetail
    {
        public int PurchaseOrderRecordId { get; set; }
        public int ReBuySequence { get; set; }
        public decimal RebuyRate { get; set; }
        public decimal ReBuyWith { get; set; }
        public string BuyType { get; set; }

        public decimal CurrencyPurchased { get; set; }

        public decimal SaleRate { get; set; }

        public int TradeProfitRateId { get; set; }
    }

    public class TradeProfitDetailResult
    {

        public int UserId { get; set; }
        public string MarketName { get; set; }
        public decimal SaleRate { get; set; }
        public int PurchaseOrderRecordId { get; set; }
        public int MarketId { get; set; }
        public int TradeProfitRateId { get; set; }
        public decimal CurrencyPurchased { get; set; }
        public decimal TickerRateForSale { get; set; }
        public decimal? MaxTickerValue { get; set; }

    }
    public class BuyStopUpDownResult
    {
        public decimal BuyStopUpPercentage { get; set; }

        public decimal BuyStopUpValue { get; set; }

        public decimal BuyStopUpRate { get; set; }

        public decimal SellRate { get; set; }

        public decimal BuyStopDownPercentage { get; set; }

        public decimal BuyStopDownValue { get; set; }

        public decimal BuyStopDownRate { get; set; }

        public int MarketId { get; set; }

        public string MarketName { get; set; }

        public int CreatorUserId { get; set; }

        public int UserSessionDetailId { get; set; }

        public int PurchaseOrderRecordId { get; set; }
        public decimal FirstBuyWith { get; set; }

    }


    public class FirstBuyOrdersRecord
    {
        public int PurchaseOrderRecordId { get; set; }        
        public int CreatorUserId { get; set; }
        public int UserSessionDetailId { get; set; }       
        public long OrderId { get; set; }
        public string MarketName { get; set; }
        public int MarketId { get; set; }
        public bool? IsSold { get; set; }
        public bool? IsPendingOrder { get; set; }
        public string OrderType { get; set; }
    }

}
