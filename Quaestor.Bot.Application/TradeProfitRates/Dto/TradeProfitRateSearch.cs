using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quaestor.Bot.TradeProfitRates.Dto
{
    public class TradeProfitRateSearch : EntityDto
    {
        public int? PurchaseOrderRecordId { get; set; }
    }
}
