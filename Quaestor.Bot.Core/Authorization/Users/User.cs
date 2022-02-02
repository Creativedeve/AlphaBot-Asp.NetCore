using System;
using Abp.Authorization.Users;
using Abp.Extensions;

namespace Quaestor.Bot.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "Q@uaestor@123";

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public virtual byte[] UserImage { get; set; }

        public virtual string UserImageType { get; set; }
        public virtual string UserImageName { get; set; }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress
            };

            user.SetNormalizedNames();

            return user;
        }
        public string DeviceType { get; set; }
        public string DeviceUUID { get; set; }
        public string DeviceFCMToken { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsTrail { get; set; }
    }
}
