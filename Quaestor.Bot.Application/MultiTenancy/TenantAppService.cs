using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Quaestor.Bot.Authorization;
using Quaestor.Bot.Authorization.Roles;
using Quaestor.Bot.Authorization.Users;
using Quaestor.Bot.Editions;
using Quaestor.Bot.MultiTenancy.Dto;
using System;

namespace Quaestor.Bot.MultiTenancy
{
    [AbpAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantAppService : AsyncCrudAppService<Tenant, TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>, ITenantAppService
    {
        private readonly TenantManager _tenantManager;
        private readonly EditionManager _editionManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;
        private readonly IPasswordHasher<User> _passwordHasher;

        public TenantAppService(
            IRepository<Tenant, int> repository,
            TenantManager tenantManager,
            EditionManager editionManager,
            UserManager userManager,
            RoleManager roleManager,
            IAbpZeroDbMigrator abpZeroDbMigrator,
            IPasswordHasher<User> passwordHasher)
            : base(repository)
        {
            _tenantManager = tenantManager;
            _editionManager = editionManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _abpZeroDbMigrator = abpZeroDbMigrator;
            _passwordHasher = passwordHasher;
        }

        public override async Task<TenantDto> Create(CreateTenantDto input)
        {
            try
            {
                CheckCreatePermission();

                // Create tenant
                var tenant = ObjectMapper.Map<Tenant>(input);
                tenant.ConnectionString = input.ConnectionString.IsNullOrEmpty()
                    ? null
                    : SimpleStringCipher.Instance.Encrypt(input.ConnectionString);

                var defaultEdition = await _editionManager.FindByNameAsync(EditionManager.DefaultEditionName);
                if (defaultEdition != null)
                {
                    tenant.EditionId = defaultEdition.Id;
                }

                await _tenantManager.CreateAsync(tenant);
                await CurrentUnitOfWork.SaveChangesAsync(); // To get new tenant's id.

                // Create tenant database
                _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);

                // We are working entities of new tenant, so changing tenant filter
                using (CurrentUnitOfWork.SetTenantId(tenant.Id))
                {
                    // Create static roles for new tenant
                    CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));

                    await CurrentUnitOfWork.SaveChangesAsync(); // To get static role ids

                    // Grant all permissions to admin role
                    var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
                    await _roleManager.GrantAllPermissionsAsync(adminRole);

                    // Create admin user for the tenant
                    var adminUser = User.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress);
                    await _userManager.InitializeOptionsAsync(tenant.Id);
                    CheckErrors(await _userManager.CreateAsync(adminUser, User.DefaultPassword));
                    await CurrentUnitOfWork.SaveChangesAsync(); // To get admin user's id

                    // Assign admin user to role!
                    CheckErrors(await _userManager.AddToRoleAsync(adminUser, adminRole.Name));
                    await CurrentUnitOfWork.SaveChangesAsync();
                }

                return MapToEntityDto(tenant);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        protected override void MapToEntity(TenantDto updateInput, Tenant entity)
        {
            try
            {
                // Manually mapped since TenantDto contains non-editable properties too.
                entity.Name = updateInput.Name;
                entity.TenancyName = updateInput.TenancyName;
                entity.IsActive = updateInput.IsActive;
            }
            catch (Exception ex)
            {
                ;
            }

        }

        public override async Task Delete(EntityDto<int> input)
        {
            try
            {
                CheckDeletePermission();

                var tenant = await _tenantManager.GetByIdAsync(input.Id);
                await _tenantManager.DeleteAsync(tenant);
            }
            catch (Exception ex)
            {
                await Task.FromResult(0);
            }

        }

        private void CheckErrors(IdentityResult identityResult)
        {
            try
            {
                identityResult.CheckErrors(LocalizationManager);
            }
            catch (Exception ex)
            {
                ;
            }

        }
    }
}
