using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.Runtime.Session;
using Quaestor.Bot.Authorization;
using Quaestor.Bot.Authorization.Roles;
using Quaestor.Bot.Authorization.Users;
using Quaestor.Bot.Roles.Dto;
using Quaestor.Bot.Users.Dto;
using System;
using Microsoft.AspNetCore.Hosting;

namespace Quaestor.Bot.Users
{
    
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private IHostingEnvironment _env;

        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IHostingEnvironment env)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _env = env;
        }

        public override async Task<UserDto> Create(CreateUserDto input)
        {

            
                CheckCreatePermission();

                var user = ObjectMapper.Map<User>(input);
                user.TenantId = AbpSession.TenantId;
                user.IsEmailConfirmed = true;

                await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            List<IdentityError> lstIdentityError = CheckErrors(await _userManager.CreateAsync(user, input.Password));

                if (input.RoleNames != null)
                {
                    CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
                }
                CurrentUnitOfWork.SaveChanges();
                return MapToEntityDto(user);
                        
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        public override async Task<UserDto> Update(UserDto input)
        {
            try
            {
                CheckUpdatePermission();
                var user = await _userManager.GetUserByIdAsync(input.Id);

                input.Password = user.Password;

                MapToEntity(input, user);

                CheckErrors(await _userManager.UpdateAsync(user));

                if (input.RoleNames != null)
                {
                    CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
                }

                return await Get(input);
            }
            catch (Exception e)
            {

                return null;
            }
            
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        public override async Task Delete(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roles = _roleManager.Roles.Where(r => user.Roles.Any(ur => ur.RoleId == r.Id)).Select(r => r.NormalizedName);
            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();
            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual List<IdentityError> CheckErrors(IdentityResult identityResult)
        {
            if (identityResult.Errors.Count() > 0)
            {
                return identityResult.Errors.ToList();
            }
            else
            {
                identityResult.CheckErrors(LocalizationManager);
                return null;
            }
        }


        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<Google2FA> UpdateIs2FaEnable(string userId)
        {
            Google2FA google2FA = new Google2FA();
            try
            {
                var user = await _userManager.GetUserByIdAsync(Convert.ToInt64(userId));
                if (user.IsTwoFactorEnabled)
                {
                    user.IsTwoFactorEnabled = false;
                }
                else
                {
                    user.IsTwoFactorEnabled = true;
                }
                CheckErrors(await _userManager.UpdateAsync(user));
                CurrentUnitOfWork.SaveChanges();
                google2FA.Status = "OK";
            }
            catch (Exception)
            {
                google2FA.Status = "Fail";
                return google2FA;
            }
            return google2FA;
        }

        // tanseef       
        public async Task<UserDto> GetUserByEmailAsync(string Email)
        {
            try
            {                
                var User = await _userManager.FindByNameOrEmailAsync(Email);
                if (User != null)
                {                                    
                    return ObjectMapper.Map<UserDto>(User);
                }
                else
                {
                    return null;
                }                
            }
            catch (Exception e)
            { 
                return null;
            }            
        }

        public async Task<string> GetUserByEmailAndSendResetPasswordLink(string Email)
        {
            string responseResult = "";
            try
            {
                
                var User = await _userManager.FindByNameOrEmailAsync(Email);
                if (User != null)
                {

                    string Encrypted = GenericFuntions.Encrypt(Email + "#" + User.Id + "#" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt"));

                    // string url = GenericFuntions.MainUrlpath + Url.Content("~") + "Home/ResetPassword?key=" + Server.UrlEncode(Encrypted);
                    string url = GenericFuntions.MainUrlpath  + "/Home/ResetPassword?key=" + System.Web.HttpUtility.UrlEncode(Encrypted);

                    string html = string.Format(GenericFuntions.ReadHtmlPage("ForgetPassword", _env.WebRootPath), url);
                    bool IsSent = await GenericFuntions.SendEmail("Change password request", html, Email);

                    if (IsSent)
                    {
                        return responseResult = "sent"; 
                    }
                    else
                    {
                        return responseResult = "senterror";
                    }                    
                }
                else
                {
                    return responseResult = "notexist";
                }
            }
            catch (Exception e)
            {
                return responseResult = "notexist"; ;
            }
        }

        public async Task<List<IdentityError>> UpdatePassword(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            //MapToEntity(input, user);
            user.Password = input.Password;
           

            List<IdentityError> lstIdentityError = CheckErrors(await _userManager.ChangePasswordAsync(user, user.Password));
            //List<IdentityError> lstIdentityError = CheckErrors(await _userManager.ChangePasswordAsync(user, input.Password, input.NewPassword));


            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
            }

            //return await Get(input);
            return lstIdentityError;           
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<List<IdentityError>> UpdatePaswordAuth(UserDto input)
        {
            CheckUpdatePermission();
            var user = await _userManager.GetUserByIdAsync(input.Id);
            //MapToEntity(input, user);
            //user.Password = input.Password;
            //CheckErrors(await _userManager.UpdateAsync(user));

            List<IdentityError> lstIdentityError = CheckErrors(await _userManager.ChangePasswordAsync(user, input.Password, input.NewPassword));
            //CheckErrors(await _userManager.ChangePasswordAsync(user, input.Password, input.NewPassword));


            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
            }

            //return await Get(input);
            return lstIdentityError;
        }

    }
}
