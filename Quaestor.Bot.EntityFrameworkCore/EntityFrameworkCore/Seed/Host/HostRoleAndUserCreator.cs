using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Quaestor.Bot.Authorization;
using Quaestor.Bot.Authorization.Roles;
using Quaestor.Bot.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;

namespace Quaestor.Bot.EntityFrameworkCore.Seed.Host
{
    public class HostRoleAndUserCreator
    {
        private readonly BotDbContext _context;

        public HostRoleAndUserCreator(BotDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateHostRoleAndUsers();
        }

        private void CreateHostRoleAndUsers()
        {
            try
            {
                // Admin role for host
                var adminRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.Admin);
                if (adminRoleForHost == null)
                {
                    adminRoleForHost = _context.Roles.Add(new Role(null, StaticRoleNames.Host.Admin, StaticRoleNames.Host.Admin) { IsStatic = true, IsDefault = true }).Entity;
                    _context.SaveChanges();
                }

                // Grant all permissions to admin role for host
                var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                    .OfType<RolePermissionSetting>()
                    .Where(p => p.TenantId == null && p.RoleId == adminRoleForHost.Id)
                    .Select(p => p.Name)
                    .ToList();

                var permissions = PermissionFinder
                    .GetAllPermissions(new BotAuthorizationProvider())
                    .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host) &&
                                !grantedPermissions.Contains(p.Name))
                    .ToList();

                if (permissions.Any())
                {
                    _context.Permissions.AddRange(
                        permissions.Select(permission => new RolePermissionSetting
                        {
                            TenantId = null,
                            Name = permission.Name,
                            IsGranted = true,
                            RoleId = adminRoleForHost.Id
                        })
                    );
                    _context.SaveChanges();
                }

                // Admin user for host
                var adminUserForHost = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == null && u.UserName == AbpUserBase.AdminUserName);
                if (adminUserForHost == null)
                {
                    var user = new User
                    {
                        TenantId = null,
                        UserName = AbpUserBase.AdminUserName,
                        Name = "admin",
                        Surname = "admin",
                        EmailAddress = "m.rehman@quaestorsolutions.com",
                        IsEmailConfirmed = true,
                        IsActive = true
                    };

                    user.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user, "123qwe");
                    user.SetNormalizedNames();

                    adminUserForHost = _context.Users.Add(user).Entity;
                    _context.SaveChanges();

                    // Assign Admin role to admin user
                    _context.UserRoles.Add(new UserRole(null, adminUserForHost.Id, adminRoleForHost.Id));
                    _context.SaveChanges();

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
