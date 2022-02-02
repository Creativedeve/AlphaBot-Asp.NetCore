using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Extensions;
using Quaestor.Bot.Validation;

namespace Quaestor.Bot.Authorization.Accounts.Dto
{
    public class RegisterInput : IValidatableObject
    {
        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        [DisableAuditing]
        public string CaptchaResponse { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool isValid = true;

            try
            {
                if (!UserName.IsNullOrEmpty())
                {
                    if (!UserName.Equals(EmailAddress) && ValidationHelper.IsEmail(UserName))
                    {
                        isValid = false;
                    }
                }

                isValid = true;
            }
            catch (Exception ex)
            {
                isValid = false;
            }

            if (isValid == false)
            {
                yield return new ValidationResult("Username cannot be an email address unless it's the same as your email address!");
            }
        }
    }
}
