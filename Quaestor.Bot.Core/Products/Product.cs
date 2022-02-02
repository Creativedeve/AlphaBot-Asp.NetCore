using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Quaestor.Bot.Products
{
    [Table("Products")]
    public class Product : CreationAuditedEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
    }
}
