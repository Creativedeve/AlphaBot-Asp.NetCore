using Abp.Domain.Entities.Auditing;
using Quaestor.Bot.Markets;
using Quaestor.Bot.PurchaseOrderRecords;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quaestor.Bot.TradeProfitRates
{

    [Table("TradeProfitRate")]
    public class TradeProfitRate : FullAuditedEntity
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

        [ForeignKey("PurchaseOrderRecordId")]
        public virtual PurchaseOrderRecord PurchaseOrderRecord { get; set; }
        public int PurchaseOrderRecordId { get; set; }

        [ForeignKey("MarketId")]
        public virtual Market Market { get; set; }
        public int MarketId { get; set; }
        public bool IsProcessed { get; set; }
        public int OrderId { get; set; }
        public bool? IsSold { get; set; }
    }
}
