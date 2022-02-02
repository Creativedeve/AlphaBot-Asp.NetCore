using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities.Auditing;
using Quaestor.Bot.Exchanges;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quaestor.Bot.TradingRules
{
    [Table("TradingParameters")]
    public class TradingParameter : CreationAuditedEntity
    {

        [ForeignKey("ExchangeId")]
        public virtual Exchange Exchange { get; set; }
        public int ExchangeId { get; set; }

        public string Pair { get; set; }
        public decimal MinTradeAmount { get; set; }
        public string MarketName { get; set; }
        public decimal MinTickSize { get; set; }
        public decimal MinOrderValue { get; set; }
    }
}
