using System;
using System.Collections.Generic;
using System.Text;

namespace Quaestor.Bot.PackagesDetail
{
    public class UserPackageExpiryResult
    {
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public decimal RemainingDays { get; set; }
    }
}
