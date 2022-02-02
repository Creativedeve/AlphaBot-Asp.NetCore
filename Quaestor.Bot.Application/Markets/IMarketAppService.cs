using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Quaestor.Bot.Markets.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quaestor.Bot.Markets
{
  public  interface IMarketAppService: IApplicationService
    {
        ListResultDto<MarketListDto>GetMarketsByExchangeId(SearchMarketInput input);
        Task<List<Market>> GetMarketsListByExchangeId(SearchMarketInput input);
    }
}
