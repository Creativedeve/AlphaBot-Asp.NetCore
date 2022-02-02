using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quaestor.Bot.UserSessions.Dto
{
    [AutoMapFrom(typeof(UserSession))]
    public class UserSessionDto:EntityDto<int>
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
