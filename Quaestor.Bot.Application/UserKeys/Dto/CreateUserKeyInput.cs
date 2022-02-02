using Abp.AutoMapper;

namespace Quaestor.Bot.UserKeys.Dto
{
    [AutoMapTo(typeof(UserKey))]
    public class CreateUserKeyInput
    {
        public string SecretKey { get; set; }
        public string ApiKey { get; set; }
        public int ExchangeId { get; set; }
        public virtual long UserId { get; set; }
    }
    public class UserKeySearchInput
    {
        public int Id { get; set; }
    }
    public class UserKeySearchByUserIdInput
    {
        public int UserId { get; set; }
    }

    //public class TradingRulesInput
    //{
    //    public string symbol { get; set; }
    //    public decimal Quantity { get; set; }
    //    public decimal? Price { get; set; }        
    //}
}
