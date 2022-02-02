using Abp.Application.Services.Dto;

namespace Quaestor.Bot.Exchanges.Dto
{
    public class ExchangeDto : EntityDto<int>
    {
        public string Name { get; set; }
    }
}
