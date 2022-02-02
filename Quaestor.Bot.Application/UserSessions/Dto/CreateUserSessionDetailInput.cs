using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Quaestor.Bot.UserSessions.Dto
{
    [AutoMapTo(typeof(UserSessionDetail))]
    public class CreateUserSessionDetailInput
    {
        public virtual int UserSessionId { get; set; }
        public virtual int MarketId { get; set; }
        public decimal BTCAllocated { get; set; }
        public decimal? TradeProfitPercentage { get; set; }
        public decimal? FirstBuyEquityPercentage { get; set; }
        public decimal? RebuyPercentage { get; set; }
        public decimal? BuyStopUp { get; set; }
        public decimal? BuyStopDown { get; set; }
        public decimal? FirstRebuy { get; set; }
        public decimal? SecondRebuy { get; set; }
        public decimal? ThirdRebuy { get; set; }
        public decimal? FourthRebuy { get; set; }
        public decimal? FifthRebuy { get; set; }
        public decimal? SixthRebuy { get; set; }
        public decimal? SeventRebuy { get; set; }
        public decimal? FirstRebuyDrop { get; set; }
        public decimal? SecondRebuyDrop { get; set; }
        public decimal? ThirdRebuyDrop { get; set; }
        public decimal? FourthRebuyDrop { get; set; }
        public decimal? FifthRebuyDrop { get; set; }
        public decimal? SixthRebuyDrop { get; set; }
        public decimal? SeventhRebuyDrop { get; set; }
        public int ExchangeId { get; set; }
    }
    public class UserSessionSearchInput : EntityDto<int>
    {
        public int? UserId { get; set; }
    }
    public class UserSessionDetailSearchInput : EntityDto<int>
    {
        public int UserSessionId { get; set; }
    }

    public class UserPurchaseOrderInput : EntityDto<int>
    {
        public int? UserId { get; set; }
    }

}
