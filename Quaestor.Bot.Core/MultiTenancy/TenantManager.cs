using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Quaestor.Bot.Authorization.Users;
using Quaestor.Bot.Editions;

namespace Quaestor.Bot.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository, 
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository, 
            EditionManager editionManager,
            IAbpZeroFeatureValueStore featureValueStore) 
            : base(
                tenantRepository, 
                tenantFeatureRepository, 
                editionManager,
                featureValueStore)
        {
        }
    }
}
