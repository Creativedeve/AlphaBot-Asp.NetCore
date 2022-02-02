using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quaestor.Bot.PurchaseOrderRecords
{
    [Table("PurchaseOrderRecordDetail")]
    public class PurchaseOrderRecordDetail : FullAuditedEntity
    {
        public int ReBuySequence { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? ReBuyRate { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? ReBuyValue { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? ReBuyWith { get; set; }

        [ForeignKey("PurchaseOrderRecordId")]
        public virtual PurchaseOrderRecord PurchaseOrderRecord { get; set; }
        public int PurchaseOrderRecordId { get; set; }
        [ForeignKey("MarketId")]
        public virtual Markets.Market Market { get; set; }
        public int MarketId { get; set; }
        public long OrderId { get; set; }
        public bool? IsSold { get; set; }
    }
}
