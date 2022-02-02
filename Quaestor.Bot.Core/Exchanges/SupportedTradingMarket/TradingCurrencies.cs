using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Quaestor.Bot.Exchanges.SupportedTradingMarket
{
    [Table("SupportedTradeCurrencies")]
    public class TradingCurrencies : FullAuditedEntity
    {
        [ForeignKey("ExchangeId")]
        public virtual Exchange Exchange { get; set; }
        public string  CurrencyName { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal MinimumValue { get; set; }

        public bool? IsActive { get; set; }
    }
}
