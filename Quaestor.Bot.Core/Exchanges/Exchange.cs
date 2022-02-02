using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Quaestor.Bot.Exchanges
{
    [Table("Exchange")]
    public class Exchange :FullAuditedEntity
    {
        public string Name { get; set; }

    }
}
