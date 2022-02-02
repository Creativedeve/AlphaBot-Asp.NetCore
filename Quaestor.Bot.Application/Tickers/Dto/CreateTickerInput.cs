using Abp.AutoMapper;
using System;

namespace Quaestor.Bot.Tickers.Dto
{
    [AutoMapTo(typeof(Ticker))]
    public class CreateTickerInput
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
