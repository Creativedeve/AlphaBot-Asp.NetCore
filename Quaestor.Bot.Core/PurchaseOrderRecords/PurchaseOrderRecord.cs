using Abp.Domain.Entities.Auditing;
using Quaestor.Bot.UserSessions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quaestor.Bot.PurchaseOrderRecords
{
    [Table("PurchaseOrderRecord")]
    public class PurchaseOrderRecord : FullAuditedEntity
    {
        [Column(TypeName = "decimal(18,10)")]
        public decimal? FirstBuyWith { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal FirstBuyRate { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? FirstBuyValue { get; set; }
        [ForeignKey("UserSessionDetailId")]
        public virtual UserSessionDetail UserSessionDetail { get; set; }
        public int UserSessionDetailId { get; set; }
        public List<PurchaseOrderRecordDetail> PurchaseOrderRecordDetails { get; set; }
        public long OrderId { get; set; }
        public string MarketName { get; set; }
        public int MarketId { get; set; }
        public bool? IsSold { get; set; }
        public bool? IsPendingOrder { get; set; }
        public string OrderType { get; set; }

    }
}
