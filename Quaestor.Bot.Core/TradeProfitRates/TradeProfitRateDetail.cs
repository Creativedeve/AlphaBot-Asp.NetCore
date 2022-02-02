using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quaestor.Bot.TradeProfitRates
{
    [Table("TradeProfitRateDetail")]
    public class TradeProfitRateDetail : FullAuditedEntity
    {
        [Column(TypeName = "decimal(18,10)")]
        public decimal? BTCInvested { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? CurrencyPurchased { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? AverageCurrencyRate { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? TradeProfitPercentageRate { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? TradeProfitSaleRate { get; set; }

        [ForeignKey("TradeProfitRateId")]
        public virtual TradeProfitRate TradeProfitRate { get; set; }
        public int TradeProfitRateId { get; set; }
        public int OrderId { get; set; }
        public int BuySequence { get; set; }
    }
}
