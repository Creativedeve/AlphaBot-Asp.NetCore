using System;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Quaestor.Bot.Authorization.Users;

namespace Quaestor.Bot.Users.Dto
{
	[AutoMapTo(typeof(User))]
	public class CreateUserDto : IShouldNormalize
	{
		[Required]
		[StringLength(AbpUserBase.MaxUserNameLength)]
		public string UserName { get; set; }

		[Required]
		[StringLength(AbpUserBase.MaxNameLength)]
		public string Name { get; set; }

		[Required]
		[StringLength(AbpUserBase.MaxSurnameLength)]
		public string Surname { get; set; }

		[Required]
		[EmailAddress]
		[StringLength(AbpUserBase.MaxEmailAddressLength)]
		public string EmailAddress { get; set; }

		public bool IsActive { get; set; }

		public string[] RoleNames { get; set; }

		[Required]
		[StringLength(AbpUserBase.MaxPlainPasswordLength)]
		[DisableAuditing]
		public string Password { get; set; }

        public byte[] UserImage { get; set; }
        public string UserImageType { get; set; }
        public string UserImageName { get; set; }

        public void Normalize()
		{
			if (RoleNames == null)
			{
				RoleNames = new string[0];
			}
		}
        public DateTime? ExpiryDate { get; set; }
    }

	public class Google2FA
	{
		public string Status { get; set; }
	}
}
