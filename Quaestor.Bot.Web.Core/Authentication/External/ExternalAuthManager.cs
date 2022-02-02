using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;

namespace Quaestor.Bot.Authentication.External
{
    public class ExternalAuthManager : IExternalAuthManager, ITransientDependency
    {
        private readonly IIocResolver _iocResolver;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;

        public ExternalAuthManager(IIocResolver iocResolver, IExternalAuthConfiguration externalAuthConfiguration)
        {
            _iocResolver = iocResolver;
            _externalAuthConfiguration = externalAuthConfiguration;
        }

        public Task<bool> IsValidUser(string provider, string providerKey, string providerAccessCode)
        {
            try
            {
                using (var providerApi = CreateProviderApi(provider))
                {
                    return providerApi.Object.IsValidUser(providerKey, providerAccessCode);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public Task<ExternalAuthUserInfo> GetUserInfo(string provider, string accessCode)
        {
            try
            {
                using (var providerApi = CreateProviderApi(provider))
                {
                    return providerApi.Object.GetUserInfo(accessCode);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public IDisposableDependencyObjectWrapper<IExternalAuthProviderApi> CreateProviderApi(string provider)
        {
            try
            {
                var providerInfo = _externalAuthConfiguration.Providers.FirstOrDefault(p => p.Name == provider);
                if (providerInfo == null)
                {
                    throw new Exception("Unknown external auth provider: " + provider);
                }

                var providerApi = _iocResolver.ResolveAsDisposable<IExternalAuthProviderApi>(providerInfo.ProviderApiType);
                providerApi.Object.Initialize(providerInfo);
                return providerApi;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
