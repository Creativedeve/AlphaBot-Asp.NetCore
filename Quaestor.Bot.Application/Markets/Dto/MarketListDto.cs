using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Quaestor.Bot.Markets.Dto
{
    [AutoMapTo(typeof(Market))]
    public class MarketListDto : EntityDto<int>
    {

        public int ExchangeId { get; set; }
        public string Name { get; set; }
    }
}
