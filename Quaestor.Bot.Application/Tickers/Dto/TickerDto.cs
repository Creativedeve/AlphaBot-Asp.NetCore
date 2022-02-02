using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quaestor.Bot.Tickers.Dto
{
    public class TickerDto : EntityDto<int>
    {
        public int ExchangeId { get; set; }
        public long Timestamp { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }

        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }
    }
}
