using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quaestor.Bot.UserSessions
{
    [Table("UserSessionDetail")]
    public class UserSessionDetail : FullAuditedEntity
    {

        [ForeignKey("UserSessionId")]
        public virtual UserSession UserSession { get; set; }
        public virtual int UserSessionId { get; set; }
        public virtual int MarketId { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? BTCAllocated { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? TradeProfitPercentage { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? FirstBuyEquityPercentage { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? RebuyPercentage { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? BuyStopUp { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? BuyStopDown { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? FirstRebuy { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? SecondRebuy { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? ThirdRebuy { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? FourthRebuy { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? FifthRebuy { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? SixthRebuy { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? SeventRebuy { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? FirstRebuyDrop { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? SecondRebuyDrop { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? ThirdRebuyDrop { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? FourthRebuyDrop { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? FifthRebuyDrop { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? SixthRebuyDrop { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? SeventhRebuyDrop { get; set; }
        public int ExchangeId { get; set; }
        public bool IsSessionClosed { get; set; }
    }
}
