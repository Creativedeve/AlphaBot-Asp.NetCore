using Abp.AutoMapper;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace Quaestor.Bot.UserSessions.Dto
{
    [AutoMapTo(typeof(UserSession))]
    public class UserSessionListDto : Entity<int>
    {
        public virtual long UserId { get; set; }
        public System.DateTime Start { get; set; }
        public System.DateTime End { get; set; }
        public decimal? BTCValueStart { get; set; }
        public decimal? BTCValueEnd { get; set; }
        public bool? IsActive { get; set; }
        public decimal? AvailableBalanceBTC { get; set; }
        public virtual List<UserSessionDetail> UserSessionDetails { get; set; }
    }
}
