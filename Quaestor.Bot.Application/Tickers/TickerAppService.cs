using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Nito.AsyncEx;
using Quaestor.Bot.Tickers.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quaestor.Bot.Tickers
{
    [AbpAuthorize]
    public class TickerAppService : BotAppServiceBase, ITickerAppService
    {
        #region Properties
        private readonly IRepository<Ticker> _tickerRepository;
        #endregion

        #region Constructor
        public TickerAppService(IRepository<Ticker> tickerRepository)
        {
            _tickerRepository = tickerRepository;
        }
        #endregion

        #region Methods
        public ListResultDto<TickerListDto> GetTickersAsync()
        {
            var ticker = _tickerRepository
                .GetAll()
                .ToList();

            return new ListResultDto<TickerListDto>(ObjectMapper.Map<List<TickerListDto>>(ticker));
        }
        public async Task CreateTicker(CreateTickerInput input)
        {
            var ticker = ObjectMapper.Map<Ticker>(input);
            await _tickerRepository.InsertAsync(ticker);
        }
        public async Task UpdateTicker(EditTickerInput input)
        {
            var ticker = await _tickerRepository.GetAsync(input.Id);
            ticker.DateTime = input.DateTime;
            ticker.Open = input.Open;
            ticker.Timestamp = input.Timestamp;
            ticker.Close = input.Close;
            ticker.OpenTime = input.OpenTime;
            ticker.CloseTime = input.CloseTime;
            await _tickerRepository.UpdateAsync(ticker);
        }

        //public async Task<DateTime> GetLastSavedDateByMarketId(int Id)
        //{
        //    var task = Task.Run(() => GetLastTickerSaveDateTime(Id));
        //    var result = await task;
        //    return result;
        //}

        //private DateTime GetLastTickerSaveDateTime(int Id)
        //{
        //    var ticker = _tickerRepository
        //        .GetAll()
        //        .Where(t => t.Id == Id)
        //       .OrderByDescending(p => p.DateTime).FirstOrDefault();

        //    return ticker.DateTime;
        //}
        #endregion
    }
}
