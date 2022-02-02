using Abp.Domain.Entities.Auditing;
using Quaestor.Bot.Exchanges;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Quaestor.Bot.Markets
{
   public class Market: FullAuditedEntity
    {
        public string Name { get; set; }

        [ForeignKey("ExchangeId")]
        public virtual Exchange Exchange { get; set; }
        public int ExchangeId { get; set; }

        public bool? IsActive { get; set; }
    }
}
