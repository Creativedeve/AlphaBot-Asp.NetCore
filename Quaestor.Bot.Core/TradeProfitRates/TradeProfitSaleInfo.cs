using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quaestor.Bot.TradeProfitRates
{
    [Table("TradeProfitSellInfo")]
    public class TradeProfitSellInfo : CreationAuditedEntity
    {
        [Column(TypeName = "decimal(18,8)")]
        public decimal? SellQuantity { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal? SellRate { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal? SellAmount { get; set; }
        public long OrderId { get; set; }
        public int TradeProfitRateId { get; set; }
        public int MarketId { get; set; }
        public int PurchaseOrderRecordId { get; set; }

        public bool? IsSellOnSessionClose { get; set; }
    }
}
