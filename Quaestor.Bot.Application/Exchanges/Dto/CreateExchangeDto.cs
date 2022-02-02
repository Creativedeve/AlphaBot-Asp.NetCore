using Abp.AutoMapper;

namespace Quaestor.Bot.Exchanges.Dto
{
    [AutoMapTo(typeof(Exchange))]
    public class CreateExchangeDto
    {
        public string Name { get; set; }
    }
    public class ExchangeSearchInput
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
