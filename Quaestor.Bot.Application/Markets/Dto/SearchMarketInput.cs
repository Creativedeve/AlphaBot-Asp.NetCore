using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Quaestor.Bot.Markets.Dto
{
    [AutoMapTo(typeof(Market))]
    public class SearchMarketInput : EntityDto
    {
        public int ExchangeId { get; set; }
    }
}
