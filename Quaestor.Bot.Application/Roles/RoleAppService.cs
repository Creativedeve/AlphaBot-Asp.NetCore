using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Extensions;
using Quaestor.Bot.Authorization;
using Quaestor.Bot.Authorization.Roles;
using Quaestor.Bot.Authorization.Users;
using Quaestor.Bot.Roles.Dto;
using System;

namespace Quaestor.Bot.Roles
{
    [AbpAuthorize(PermissionNames.Pages_Roles)]
    public class RoleAppService : AsyncCrudAppService<Role, RoleDto, int, PagedResultRequestDto, CreateRoleDto, RoleDto>, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;

        public RoleAppService(IRepository<Role> repository, RoleManager roleManager, UserManager userManager)
            : base(repository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public override async Task<RoleDto> Create(CreateRoleDto input)
        {
            try
            {
                CheckCreatePermission();

                var role = ObjectMapper.Map<Role>(input);
                role.SetNormalizedName();

                CheckErrors(await _roleManager.CreateAsync(role));

                var grantedPermissions = PermissionManager
                    .GetAllPermissions()
                    .Where(p => input.Permissions.Contains(p.Name))
                    .ToList();

                await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

                return MapToEntityDto(role);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<ListResultDto<RoleListDto>> GetRolesAsync(GetRolesInput input)
        {
            try
            {
                var roles = await _roleManager
               .Roles
               .WhereIf(
                   !input.Permission.IsNullOrWhiteSpace(),
                   r => r.Permissions.Any(rp => rp.Name == input.Permission && rp.IsGranted)
               )
               .ToListAsync();

                return new ListResultDto<RoleListDto>(ObjectMapper.Map<List<RoleListDto>>(roles));
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public override async Task<RoleDto> Update(RoleDto input)
        {
            try
            {
                CheckUpdatePermission();

                var role = await _roleManager.GetRoleByIdAsync(input.Id);

                ObjectMapper.Map(input, role);

                CheckErrors(await _roleManager.UpdateAsync(role));

                var grantedPermissions = PermissionManager
                    .GetAllPermissions()
                    .Where(p => input.Permissions.Contains(p.Name))
                    .ToList();

                await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

                return MapToEntityDto(role);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public override async Task Delete(EntityDto<int> input)
        {
            try
            {
                CheckDeletePermission();

                var role = await _roleManager.FindByIdAsync(input.Id.ToString());
                var users = await _userManager.GetUsersInRoleAsync(role.NormalizedName);

                foreach (var user in users)
                {
                    CheckErrors(await _userManager.RemoveFromRoleAsync(user, role.NormalizedName));
                }

                CheckErrors(await _roleManager.DeleteAsync(role));
            }
            catch (Exception ex)
            {
                await Task.FromResult(0);
            }

        }

        public Task<ListResultDto<PermissionDto>> GetAllPermissions()
        {
            try
            {
                var permissions = PermissionManager.GetAllPermissions();

                return Task.FromResult(new ListResultDto<PermissionDto>(
                    ObjectMapper.Map<List<PermissionDto>>(permissions)
                ));
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        protected override IQueryable<Role> CreateFilteredQuery(PagedResultRequestDto input)
        {
            try
            {
                return Repository.GetAllIncluding(x => x.Permissions);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        protected override async Task<Role> GetEntityByIdAsync(int id)
        {
            try
            {
                return await Repository.GetAllIncluding(x => x.Permissions).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected override IQueryable<Role> ApplySorting(IQueryable<Role> query, PagedResultRequestDto input)
        {
            try
            {
                return query.OrderBy(r => r.DisplayName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
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

        public async Task<GetRoleForEditOutput> GetRoleForEdit(EntityDto input)
        {
            try
            {
                var permissions = PermissionManager.GetAllPermissions();
                var role = await _roleManager.GetRoleByIdAsync(input.Id);
                var grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).ToArray();
                var roleEditDto = ObjectMapper.Map<RoleEditDto>(role);

                return new GetRoleForEditOutput
                {
                    Role = roleEditDto,
                    Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList(),
                    GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
                };
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
