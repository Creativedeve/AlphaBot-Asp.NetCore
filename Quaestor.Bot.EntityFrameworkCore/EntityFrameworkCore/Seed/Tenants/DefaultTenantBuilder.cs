using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.MultiTenancy;
using Quaestor.Bot.Editions;
using Quaestor.Bot.MultiTenancy;
using System;

namespace Quaestor.Bot.EntityFrameworkCore.Seed.Tenants
{
    public class DefaultTenantBuilder
    {
        private readonly BotDbContext _context;

        public DefaultTenantBuilder(BotDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateDefaultTenant();
        }

        private void CreateDefaultTenant()
        {
            try
            {
                // Default tenant
                var defaultTenant = _context.Tenants.IgnoreQueryFilters().FirstOrDefault(t => t.TenancyName == AbpTenantBase.DefaultTenantName);

                if (defaultTenant == null)
                {
                    defaultTenant = new Tenant(AbpTenantBase.DefaultTenantName, AbpTenantBase.DefaultTenantName);

                    var defaultEdition = _context.Editions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
                    if (defaultEdition != null)
                    {
                        defaultTenant.EditionId = defaultEdition.Id;
                    }

                    _context.Tenants.Add(defaultTenant);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ;
            }

        }
    }
}
