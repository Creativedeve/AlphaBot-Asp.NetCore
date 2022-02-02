using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.Collections.Generic;

namespace Quaestor.Bot.UserSessions.Dto
{
    [AutoMapTo(typeof(UserSession))]
    public class CreateUserSessionInput : EntityDto
    {

        public virtual long UserId { get; set; }
        public System.DateTime Start { get; set; }
        public System.DateTime End { get; set; }
        public decimal? BTCValueStart { get; set; }
        public decimal? BTCValueEnd { get; set; }
        public bool? IsActive { get; set; }
        public decimal? AvailableBalanceBTC { get; set; }
        public virtual List<CreateUserSessionDetailInput> UserSessionDetails { get; set; }
    }
    public class DeleteInput : EntityDto<int>
    {
        public int UserSessionDetailId { get; set; }
        public int UserId { get; set; }
    }

    public class UserSessionStartInput:EntityDto<int>
    {
        public int UserId { get; set; }
    }

    public class UserSessionClosePosition : EntityDto<int>
    {
        public int UserId { get; set; }
        public int MarketId { get; set; }
        public bool IsSaleImmediately { get; set; }

    }
}
