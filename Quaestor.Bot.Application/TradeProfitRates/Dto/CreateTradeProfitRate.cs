using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Quaestor.Bot.TradeProfitRates.Dto
{
    [AutoMapTo(typeof(TradeProfitRates.TradeProfitRate))]
    public class CreateTradeProfitRate:EntityDto<int>
    {
        public decimal? BTCInvested { get; set; }
        public decimal? CurrencyPurchased { get; set; }
        public decimal? AverageCurrencyRate { get; set; }
        public decimal? TradeProfitPercentageRate { get; set; }
        public decimal? TradeProfitSaleRate { get; set; }
        public int PurchaseOrderRecordId { get; set; }
    }
}
