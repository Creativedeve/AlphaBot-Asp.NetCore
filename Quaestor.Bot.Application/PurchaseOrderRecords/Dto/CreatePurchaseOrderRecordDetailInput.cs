using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quaestor.Bot.PurchaseOrderRecords.Dto
{
  public  class CreatePurchaseOrderRecordDetailInput:EntityDto<int>
    {
        public int ReBuySequence { get; set; }
        public decimal? ReBuyRate { get; set; }
        public decimal? ReBuyValue { get; set; }
        public decimal? ReBuyWith { get; set; }
        public int PurchaseOrderRecordId { get; set; }
    }
}
