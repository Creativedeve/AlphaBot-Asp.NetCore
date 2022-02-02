using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Quaestor.Bot.Products
{
    [Table("UserProducts")]
    public class UserProducts: FullAuditedEntity
    {   
        [ForeignKey("ProductId")]
        public virtual Product ProductDetail { get; set; }
        public int ProductId { get; set; }
    }
}
