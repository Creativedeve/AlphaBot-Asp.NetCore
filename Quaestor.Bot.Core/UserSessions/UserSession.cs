using Abp.Domain.Entities.Auditing;
using Quaestor.Bot.Authorization.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quaestor.Bot.UserSessions
{
    [Table("UserSessions")]
    public class UserSession : FullAuditedEntity
    {
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public virtual long UserId { get; set; }
        public System.DateTime Start { get; set; }
        public System.DateTime End { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? BTCValueStart { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? BTCValueEnd { get; set; }
        public bool? IsActive { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal? AvailableBalanceBTC { get; set; }
        public bool? IsSessionClose { get; set; }
        public bool? IsSessionInProgress { get; set; }

        public List<UserSessionDetail> UserSessionDetails { get; set; }
    }
}
