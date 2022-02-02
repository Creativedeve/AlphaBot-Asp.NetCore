using Abp.Application.Services;
using Abp.Domain.Repositories;
using Quaestor.Bot.TradeProfitRates.Dto;
using System;
using System.Threading.Tasks;

namespace Quaestor.Bot.TradeProfitRates
{
    public class TradeProfitRateAppService : AsyncCrudAppService<TradeProfitRate, TradeProfitRateDto, int, TradeProfitRateSearch, CreateTradeProfitRate, EditTradeProfitRate, TradeProfitRateSearch>, ITradeProfitRateAppService
    {
        #region Properties
        private readonly IRepository<TradeProfitRate, int> _tradeProfitRate;
        #endregion

        #region Constructor
        public TradeProfitRateAppService(IRepository<TradeProfitRate, int> repository)
          : base(repository)
        {
            _tradeProfitRate = repository;
        }
        #endregion

        #region Methods
        public override async Task<TradeProfitRateDto> Create(CreateTradeProfitRate input)
        {
            try
            {
                var tradeProfitRate = ObjectMapper.Map<TradeProfitRate>(input);
                await _tradeProfitRate.InsertAsync(tradeProfitRate); ;
                await CurrentUnitOfWork.SaveChangesAsync();
                var Order = ObjectMapper.Map<TradeProfitRateDto>(tradeProfitRate);
                return Order;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public override async Task<TradeProfitRateDto> Update(EditTradeProfitRate input)
        {
            try
            {
                var tradeProfitRate = await _tradeProfitRate.GetAsync(input.Id);
                MapToEntity(input, tradeProfitRate);
                await _tradeProfitRate.UpdateAsync(tradeProfitRate);
                await CurrentUnitOfWork.SaveChangesAsync();
                var result = await _tradeProfitRate.GetAsync(input.Id);
                return ObjectMapper.Map<TradeProfitRateDto>(result);
            }
            catch (Exception ex)
            {
                return null;
            }


        }
        public override async Task<TradeProfitRateDto> Get(TradeProfitRateSearch input)
        {
            try
            {
                var purchaseOrder = await _tradeProfitRate.GetAsync(input.Id);
                return ObjectMapper.Map<TradeProfitRateDto>(purchaseOrder);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion
    }
}
