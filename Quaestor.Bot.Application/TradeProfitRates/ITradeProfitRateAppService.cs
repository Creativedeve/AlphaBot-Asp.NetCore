using Abp.Application.Services;
using Quaestor.Bot.TradeProfitRates.Dto;
using System.Threading.Tasks;

namespace Quaestor.Bot.TradeProfitRates
{
    public interface ITradeProfitRateAppService:IApplicationService
    {
        Task<TradeProfitRateDto> Get(TradeProfitRateSearch Input);
        Task<TradeProfitRateDto> Create(CreateTradeProfitRate input);
        Task<TradeProfitRateDto> Update(EditTradeProfitRate input);

    }
}
