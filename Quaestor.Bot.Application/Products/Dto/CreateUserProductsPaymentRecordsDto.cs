using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quaestor.Bot.Products.Dto
{
    [AutoMapTo(typeof(UserProductsPaymentRecords))]
    public class CreateUserProductsPaymentRecordsDto: EntityDto
    {
       // public virtual UserProducts UserProductsDetail { get; set; }
        public int UserProductId { get; set; }
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string SourceCurrency { get; set; }       
        public decimal SourceAmount { get; set; }

        public string DestinationCurrency { get; set; }       
        public decimal DestinationAmount { get; set; }      
        public decimal ActualAmountPaid { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime ConfirmedAt { get; set; }
        public string Type { get; set; }
        public int CreatorUserId { get; set; }
    }
    public class UserProductsPaymentRecordSearch
    {
        public string Code { get; set; }
    }
}
