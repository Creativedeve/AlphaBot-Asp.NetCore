using System;
using System.Collections.Generic;
using System.Text;

namespace Quaestor.Bot.UserSessions
{
   public class ActiveUserSession
    {
        public int UserSessionDetailId { get; set; }
        public string MarketName { get; set; }
        public decimal BTCAllocated { get; set; }
        public decimal RebuyValue { get; set; }
        public int RebuyNumber { get; set; }
    }
}
