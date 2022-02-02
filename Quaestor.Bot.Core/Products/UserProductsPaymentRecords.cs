using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Quaestor.Bot.Products
{
    [Table("UserProductsPaymentRecords")]
    public class UserProductsPaymentRecords: FullAuditedEntity
    {
        //[ForeignKey("UserProductId")]
       // public virtual UserProducts UserProductsDetail { get; set; }
        public int UserProductId { get; set; }
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string SourceCurrency { get; set; }

        [Column(TypeName = "decimal(18,10)")]
        public decimal SourceAmount { get; set; }

        public string DestinationCurrency { get; set; }

        [Column(TypeName = "decimal(18,10)")]
        public decimal DestinationAmount { get; set; }

        [Column(TypeName = "decimal(18,10)")]
        public decimal ActualAmountPaid { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime ConfirmedAt { get; set; }
        public string Type { get; set; }
    }
}
