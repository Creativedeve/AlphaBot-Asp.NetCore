using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Quaestor.Bot.Tickers.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quaestor.Bot.Tickers
{
    public interface ITickerAppService:IApplicationService
    {
        ListResultDto<TickerListDto> GetTickersAsync();
        Task CreateTicker(CreateTickerInput input);
        Task UpdateTicker(EditTickerInput input);
        //Task<DateTime> GetLastSavedDateByMarketId(TickerSearchInput input);

        
    }
}
