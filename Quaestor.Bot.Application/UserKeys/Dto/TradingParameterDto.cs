using System;
using System.Collections.Generic;
using System.Text;

namespace Quaestor.Bot.UserKeys.Dto
{
    public class TradingParameterDto
    {
        public int ExchangeId { get; set; }
        public string Pair { get; set; }
        public decimal MinTradeAmount { get; set; }
        public string MarketName { get; set; }
        public decimal MinTickSize { get; set; }
        public decimal MinOrderValue { get; set; }
        public decimal TickerValue { get; set; }
    }



    public class TradingParameterInput
    {
        //public int ExchangeId { get; set; }
        //public string Pair { get; set; }
        //public int UserId { get; set; }

        public int UserId { get; set; }
        public string Pair { get; set; }
        public decimal quantity { get; set; }
        public decimal? price { get; set; }




    }


}
