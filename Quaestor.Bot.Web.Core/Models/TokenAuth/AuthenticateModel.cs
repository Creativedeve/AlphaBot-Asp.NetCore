using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace Quaestor.Bot.Models.TokenAuth
{
    public class AuthenticateModel
    {
        public AuthenticateModel()
        {
            UserDeviceInfo = new UserDeviceInfo();
        }
        [Required]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string UserNameOrEmailAddress { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        public string Password { get; set; }
        
        public bool RememberClient { get; set; }
		public string GoogleCodeDigit { get; set; }
		public int isCleared { get; set; }
        public UserDeviceInfo UserDeviceInfo { get; set; }
    }
}
