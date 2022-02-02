using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Quaestor.Bot.Markets.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quaestor.Bot.Markets
{
    [AbpAuthorize]
    public class MarketAppService : BotAppServiceBase, IMarketAppService
    {
        #region Properties
        private readonly IRepository<Market, int> _marketRepository;
        #endregion

        #region Constructor
        public MarketAppService(IRepository<Market, int> repository)
        {
            _marketRepository = repository;
        }
        #endregion

        #region Methods
        public ListResultDto<MarketListDto> GetMarketsByExchangeId(SearchMarketInput input)
        {
            try
            {
                var markets = _marketRepository
              .GetAll()
              .Where(

                  r => r.ExchangeId == input.ExchangeId && r.IsActive!=false
              )
              .ToList().OrderBy(r => r.Name);

                return new ListResultDto<MarketListDto>(ObjectMapper.Map<List<MarketListDto>>(markets));
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<Market>> GetMarketsListByExchangeId(SearchMarketInput input)
        {
            var markets = await _marketRepository
                .GetAllListAsync(r => r.ExchangeId == input.ExchangeId && r.IsActive != false);


            return new List<Market>(ObjectMapper.Map<List<Market>>(markets));
        }
        #endregion
    }
}
