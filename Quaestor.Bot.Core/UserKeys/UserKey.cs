using Abp.Domain.Entities.Auditing;
using Quaestor.Bot.Authorization.Users;
using Quaestor.Bot.Exchanges;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quaestor.Bot.UserKeys
{
    [Table("UserKeys")]
    public class UserKey : FullAuditedEntity
    {
        public string SecretKey { get; set; }

        public string ApiKey { get; set; }
       
        [ForeignKey("ExchangeId")]
        public virtual Exchange Exchange { get; set; }
        public  int ExchangeId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public virtual long UserId { get; set; }
    }
}
