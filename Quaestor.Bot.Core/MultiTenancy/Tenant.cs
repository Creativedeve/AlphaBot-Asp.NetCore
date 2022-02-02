using Abp.MultiTenancy;
using Quaestor.Bot.Authorization.Users;

namespace Quaestor.Bot.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
