using Abp.Domain.Entities.Auditing;
using Quaestor.Bot.PurchaseOrderRecords;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quaestor.Bot.PreserveBuyInformation
{
    [Table("PreserveBuyInfo")]
    public class PreserveBuyInfo : FullAuditedEntity
    {
        [ForeignKey("PurchaseOrderRecordId")]
        public virtual PurchaseOrderRecord PurchaseOrderRecord { get; set; }
        public virtual int? PurchaseOrderRecordId { get; set; }
        public int? UserSessionDetailId { get; set; }
        public string BuyType { get; set; }
        public bool IsProcessed { get; set; }
        public int? ReBuySequence { get; set; }
        public int MarketId { get; set; }
        public int? TradeProfitId { get; set; }
        public bool? IsSold { get; set; }
    }
}
