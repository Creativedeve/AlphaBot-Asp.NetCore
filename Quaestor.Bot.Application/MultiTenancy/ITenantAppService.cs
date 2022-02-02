using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Quaestor.Bot.MultiTenancy.Dto;

namespace Quaestor.Bot.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
