using Abp.AutoMapper;

namespace Quaestor.Bot.UserKeys.Dto
{
    [AutoMapTo(typeof(UserKey))]
    public class EditUserKeyInput
    {
        public int Id { get; set; }
        public string SecretKey { get; set; }
        public string ApiKey { get; set; }
        public int ExchangeId { get; set; }
        public long UserId { get; set; }
    }
    public class BalanceInfo
    {

        public string AvailableBalance { get; set; }
        public string Status { get; set; }
    }
}
