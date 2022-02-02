using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Quaestor.Bot.PurchaseOrderRecords;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quaestor.Bot.PurchaseOrderRecords.Dto
{
    [AutoMapTo(typeof(PurchaseOrderRecords.PurchaseOrderRecord))]
    public class CreatePurchaseOrderRecordInput:EntityDto<int>
    {
        public decimal? FirstBuyWith { get; set; }
        public decimal FirstBuyRate { get; set; }
        public decimal? FirstBuyValue { get; set; }  
        public int UserSessionDetailId { get; set; }
        public List<CreatePurchaseOrderRecordDetailInput> PurchaseOrderRecordDetails { get; set; }
    }
    public class DeleteInput:EntityDto<int>
    {
        
    }
}
