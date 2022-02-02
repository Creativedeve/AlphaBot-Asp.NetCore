using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Quaestor.Bot.Exchanges.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quaestor.Bot.Exchanges
{
    
    public interface IExchangeAppService: IApplicationService
    {
        ListResultDto<ExchangeListDto> GetExchangesAsync();
        Task CreateExchange(CreateExchangeDto input);
        Task <Exchange> GetExchangeByNameAsync(ExchangeSearchInput input);
    }
}
