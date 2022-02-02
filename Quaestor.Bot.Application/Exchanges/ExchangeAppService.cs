using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Quaestor.Bot.Exchanges.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quaestor.Bot.Exchanges
{
    [AbpAuthorize]
    public class ExchangeAppService : BotAppServiceBase, IExchangeAppService
    {
        #region Properties
        private readonly IRepository<Exchange> _exchangeRepository;

        #endregion

        #region Constructor
        public ExchangeAppService(IRepository<Exchange> exchangeRepository)
        {
            _exchangeRepository = exchangeRepository;
        }
        #endregion

        #region Methods
        public ListResultDto<ExchangeListDto> GetExchangesAsync()
        {
            try
            {
                var exchange = _exchangeRepository
               .GetAll()
               .OrderBy(p => p.Name)
               .ThenBy(p => p.Name)
               .ToList();

                return new ListResultDto<ExchangeListDto>(ObjectMapper.Map<List<ExchangeListDto>>(exchange));
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<Exchange> GetExchangeByNameAsync(ExchangeSearchInput input)
        {
            Exchange result = new Exchange();
            var exchange = await _exchangeRepository
                .GetAsync(input.Id);

            if (exchange != null)
            {
                result = _exchangeRepository.GetAll().Where(e => e.Name == input.Name).FirstOrDefault();


            }
            return result;
        }
        public async Task CreateExchange(CreateExchangeDto input)
        {
            try
            {
                var excange = ObjectMapper.Map<Exchange>(input);
                await _exchangeRepository.InsertAsync(excange);
            }
            catch (Exception ex)
            {
                await Task.FromResult(0);
            }

        }
        #endregion
    }
}
