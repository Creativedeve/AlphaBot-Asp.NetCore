using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.UI;
using Quaestor.Bot.Authentication.External;
using Quaestor.Bot.Authentication.JwtBearer;
using Quaestor.Bot.Authorization;
using Quaestor.Bot.Authorization.Users;
using Quaestor.Bot.Models.TokenAuth;
using Quaestor.Bot.MultiTenancy;
using Abp.Dependency;
using Quaestor.MLM.Data.Core.Managers;

namespace Quaestor.Bot.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : BotControllerBase
    {
        private readonly LogInManager _logInManager;
        private readonly ITenantCache _tenantCache;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly TokenAuthConfiguration _configuration;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        private readonly IExternalAuthManager _externalAuthManager;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly UserManager _userManager;

        public TokenAuthController(
            LogInManager logInManager,
            ITenantCache tenantCache,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            TokenAuthConfiguration configuration,
            IExternalAuthConfiguration externalAuthConfiguration,
            IExternalAuthManager externalAuthManager,
            UserRegistrationManager userRegistrationManager,
            UserManager userManager)
        {
            _logInManager = logInManager;
            _tenantCache = tenantCache;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _configuration = configuration;
            _externalAuthConfiguration = externalAuthConfiguration;
            _externalAuthManager = externalAuthManager;
            _userRegistrationManager = userRegistrationManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModel model)
        {
            var loginResult = await GetLoginResultAsync(
                model.UserNameOrEmailAddress,
                model.Password,
                GetTenancyNameOrNull(),
                model.UserDeviceInfo
            );

            if (loginResult.User.IsTwoFactorEnabled && model.isCleared!=1)
            {
                return new AuthenticateResultModel
                {
                    IsGoogle2Fa = loginResult.User.IsTwoFactorEnabled,
                    UserId = loginResult.User.Id,
					UserName=loginResult.User.UserName
	
                };
            }
			
			var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));

            return new AuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                UserId = loginResult.User.Id,
				IsGoogle2Fa = loginResult.User.IsTwoFactorEnabled
			};
        }

        [HttpGet]
        public List<ExternalLoginProviderInfoModel> GetExternalAuthenticationProviders()
        {
            return ObjectMapper.Map<List<ExternalLoginProviderInfoModel>>(_externalAuthConfiguration.Providers);
        }

        [HttpPost]
        public async Task<ExternalAuthenticateResultModel> ExternalAuthenticate([FromBody] ExternalAuthenticateModel model)
        {
            try
            {
                var externalUser = await GetExternalUserInfo(model);

                var loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());

                switch (loginResult.Result)
                {
                    case AbpLoginResultType.Success:
                        {
                            var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));
                            return new ExternalAuthenticateResultModel
                            {
                                AccessToken = accessToken,
                                EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
                            };
                        }
                    case AbpLoginResultType.UnknownExternalLogin:
                        {
                            var newUser = await RegisterExternalUserAsync(externalUser);
                            if (!newUser.IsActive)
                            {
                                return new ExternalAuthenticateResultModel
                                {
                                    WaitingForActivation = true
                                };
                            }

                            // Try to login again with newly registered user!
                            loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());
                            if (loginResult.Result != AbpLoginResultType.Success)
                            {
                                throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                                    loginResult.Result,
                                    model.ProviderKey,
                                    GetTenancyNameOrNull()
                                );
                            }

                            return new ExternalAuthenticateResultModel
                            {
                                AccessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity)),
                                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
                            };
                        }
                    default:
                        {
                            throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                                loginResult.Result,
                                model.ProviderKey,
                                GetTenancyNameOrNull()
                            );
                        }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private async Task<User> RegisterExternalUserAsync(ExternalAuthUserInfo externalUser)
        {
            try
            {
                var user = await _userRegistrationManager.RegisterAsync(
              externalUser.Name,
              externalUser.Surname,
              externalUser.EmailAddress,
              externalUser.EmailAddress,
              Authorization.Users.User.CreateRandomPassword(),
              true
          );

                user.Logins = new List<UserLogin>
            {
                new UserLogin
                {
                    LoginProvider = externalUser.Provider,
                    ProviderKey = externalUser.ProviderKey,
                    TenantId = user.TenantId
                }
            };

                await CurrentUnitOfWork.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private async Task<ExternalAuthUserInfo> GetExternalUserInfo(ExternalAuthenticateModel model)
        {
            try
            {
                var userInfo = await _externalAuthManager.GetUserInfo(model.AuthProvider, model.ProviderAccessCode);
                if (userInfo.ProviderKey != model.ProviderKey)
                {
                    throw new UserFriendlyException(L("CouldNotValidateExternalUser"));
                }

                return userInfo;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private string GetTenancyNameOrNull()
        {
            try
            {
                if (!AbpSession.TenantId.HasValue)
                {
                    return null;
                }

                return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        //private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName, UserDeviceInfo userDeviceInfo)
        //{
        //    try
        //    {
        //        var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);
        //        Quaestor.Bot.Users.Dto.UserDto botUserResult = null;
        //        MLM.Data.Core.Managers.CoreUserManager coreUserManager = new MLM.Data.Core.Managers.CoreUserManager();

        //        bool bRes = true;
        //        if (loginResult.Result != AbpLoginResultType.Success)
        //        {
        //            var mlmUserResult = coreUserManager.GetMLMUser(usernameOrEmailAddress, password);
        //            if (mlmUserResult != null && !string.IsNullOrEmpty(mlmUserResult.UserName) && !string.IsNullOrEmpty(mlmUserResult.Document) && mlmUserResult.Document.ToLower().Trim() == "verified" && mlmUserResult.IsAccountfreezed != true && mlmUserResult.IsRefund != true)
        //            {
        //                var userExist = _userManager.Users.Where(x => x.UserName.ToLower() == mlmUserResult.UserName.ToLower()).FirstOrDefault();
        //                //var userExist = await IocManager.Instance.Resolve<Users.UserAppService>().GetUserByEmailAsync(mlmUserResult.UserName);
        //                if (userExist != null)
        //                {
        //                    //var updateduser = await _userManager.ChangePasswordAsync(userExist,mlmUserResult.Password);
        //                    //if (updateduser != null && updateduser.Succeeded)
        //                    //{
        //                    //    loginResult = await _logInManager.LoginAsync(userExist.UserName, userExist.Password, tenancyName);
        //                    //}

        //                }
        //                else
        //                {
        //                    string[] RoleNames = { "admin" };
        //                    string FirstName = mlmUserResult.Fname;
        //                    string LastName = mlmUserResult.Lname;
        //                    if (string.IsNullOrEmpty(mlmUserResult.Fname))
        //                    {
        //                        FirstName = " NILL";
        //                    }
        //                    if (string.IsNullOrEmpty(mlmUserResult.Lname))
        //                    {
        //                        LastName = " NILL";
        //                    }
        //                    Users.Dto.CreateUserDto createUserDto = new Users.Dto.CreateUserDto
        //                    {
        //                        UserName = mlmUserResult.UserName,
        //                        Name = FirstName,
        //                        Surname = LastName,
        //                        EmailAddress = mlmUserResult.Email,
        //                        IsActive = true,
        //                        RoleNames = RoleNames,
        //                        Password = mlmUserResult.Password
        //                    };
        //                    botUserResult = await IocManager.Instance.Resolve<Users.UserAppService>().Create(createUserDto);
        //                    if (botUserResult != null && !string.IsNullOrEmpty(botUserResult.UserName))
        //                    {
        //                        loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);
        //                        bRes = false;
        //                    }
        //                }

        //            }
        //        }

        //        // here we are getting device info
        //        if (bRes && usernameOrEmailAddress!="admin")
        //        {
        //            var mlmUserResult = coreUserManager.GetMLMUser(usernameOrEmailAddress, password);
        //            if (!(mlmUserResult != null && !string.IsNullOrEmpty(mlmUserResult.UserName) && !string.IsNullOrEmpty(mlmUserResult.Document) && mlmUserResult.Document.ToLower().Trim() == "verified" && mlmUserResult.IsAccountfreezed != true && mlmUserResult.IsRefund != true))
        //            {
        //               // throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
        //            }
        //        }
        //        switch (loginResult.Result)
        //        {
        //            case AbpLoginResultType.Success:
        //                {
        //                    if (userDeviceInfo != null && !string.IsNullOrEmpty(userDeviceInfo.DeviceType))
        //                    {
        //                        loginResult.User.DeviceType = userDeviceInfo.DeviceType;
        //                        loginResult.User.DeviceUUID = userDeviceInfo.DeviceUUID;
        //                        loginResult.User.DeviceFCMToken = userDeviceInfo.DeviceFCMToken;
        //                        var user = await _userManager.UpdateAsync(loginResult.User);

        //                    }
        //                    return loginResult;
        //                }
        //            default:
        //                throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName, UserDeviceInfo userDeviceInfo)
        {
            try
            {
                AbpLoginResult<Tenant, User> loginResult = null;
                var userExist = _userManager.Users.Where(x => x.UserName.ToLower() == usernameOrEmailAddress.ToLower()).FirstOrDefault();
                PasswordVerificationResult passwordVerification = PasswordVerificationResult.Failed;
                if (userExist!=null)
                {
                    passwordVerification= new PasswordHasher<User>().VerifyHashedPassword(userExist, userExist.Password, password);

                }
                if (passwordVerification == PasswordVerificationResult.Success)
                {
                    loginResult = await _logInManager.LoginAsync(userExist.UserName, password, tenancyName);
                }
                MLM.Data.Core.Managers.CoreUserManager coreUserManager = new MLM.Data.Core.Managers.CoreUserManager();
                bool bRes = true;
                if (passwordVerification != PasswordVerificationResult.Success /*loginResult.Result != AbpLoginResultType.Success*/)
                {
                var mlmUserResult = coreUserManager.GetMLMUser(usernameOrEmailAddress, password);
                if (mlmUserResult != null && !string.IsNullOrEmpty(mlmUserResult.UserName) && !string.IsNullOrEmpty(mlmUserResult.Document) && mlmUserResult.Document.ToLower().Trim() == "verified" && mlmUserResult.IsAccountfreezed != true && mlmUserResult.IsRefund != true)
                {
                    string FirstName = mlmUserResult.Fname;
                    string LastName = mlmUserResult.Lname;
                    if (string.IsNullOrEmpty(mlmUserResult.Fname))
                    {
                        FirstName = " NILL";
                    }
                    if (string.IsNullOrEmpty(mlmUserResult.Lname))
                    {
                        LastName = " NILL";
                    }
                    if (userExist != null)
                    {
                        //userExist.Name = FirstName;
                        //userExist.Surname = LastName;
                        userExist.Password = new PasswordHasher<User>().HashPassword(userExist, mlmUserResult.Password);
                        var updateduser = await _userManager.UpdateAsync(userExist);
                        if (updateduser != null && updateduser.Succeeded)
                        {
                            loginResult = await _logInManager.LoginAsync(userExist.UserName, password, tenancyName);
                        }
                    }
                    else
                    {
                        string[] RoleNames = { "admin" };
                        Users.Dto.CreateUserDto createUserDto = new Users.Dto.CreateUserDto
                        {
                            UserName = mlmUserResult.UserName,
                            Name = FirstName,
                            Surname = LastName,
                            EmailAddress = mlmUserResult.Email,
                            IsActive = true,
                            RoleNames = RoleNames,
                            Password = mlmUserResult.Password
                        };
                        var botUserResult = await IocManager.Instance.Resolve<Users.UserAppService>().Create(createUserDto);
                        if (botUserResult != null && !string.IsNullOrEmpty(botUserResult.UserName))
                        {
                            loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);
                            bRes = false;
                        }
                    }
                 }
                }
                // here we are getting device info
                if (bRes && usernameOrEmailAddress != "admin")
                {
                    var mlmUserResult = coreUserManager.GetMLMUser(usernameOrEmailAddress, password);
                    if (!(mlmUserResult != null && !string.IsNullOrEmpty(mlmUserResult.UserName) && !string.IsNullOrEmpty(mlmUserResult.Document) && mlmUserResult.Document.ToLower().Trim() == "verified" && mlmUserResult.IsAccountfreezed != true && mlmUserResult.IsRefund != true))
                    {
                        // throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
                    }
                }
                switch (loginResult.Result)
                {
                    case AbpLoginResultType.Success:
                        {
                            if (userDeviceInfo != null && !string.IsNullOrEmpty(userDeviceInfo.DeviceType))
                            {
                                loginResult.User.DeviceType = userDeviceInfo.DeviceType;
                                loginResult.User.DeviceUUID = userDeviceInfo.DeviceUUID;
                                loginResult.User.DeviceFCMToken = userDeviceInfo.DeviceFCMToken;
                                var user = await _userManager.UpdateAsync(loginResult.User);

                            }
                            return loginResult;
                        }
                    default:
                        throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            try
            {
                var now = DateTime.UtcNow;

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _configuration.Issuer,
                    audience: _configuration.Audience,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(expiration ?? _configuration.Expiration),
                    signingCredentials: _configuration.SigningCredentials
                );

                return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private static List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            try
            {
                var claims = identity.Claims.ToList();
                var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

                // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
                claims.AddRange(new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

                return claims;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private string GetEncrpyedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<bool> UnAuthenticate(int  UserId)
        {
            bool res = false;
            var identity = await _userManager.GetUserByIdAsync(UserId);
            if (identity!=null && !string.IsNullOrEmpty(identity.DeviceFCMToken))
            {
                identity.DeviceFCMToken = string.Empty;
                identity.DeviceType = string.Empty;
                identity.DeviceUUID = string.Empty;
                await _userManager.UpdateAsync(identity);
                res = true;
            }
            return res;
        }
    }
}
