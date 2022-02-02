using Abp.Domain.Entities.Auditing;
using Quaestor.Bot.Exchanges;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quaestor.Bot.Tickers
{
    [Table("Tickers")]
    public class Ticker : FullAuditedEntity
    {

        //[Required]
        [ForeignKey("ExchangeId")]
        public virtual Exchange Exchange { get; set; }
        public int ExchangeId { get; set; }
        public long Timestamp { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }
    }
}
