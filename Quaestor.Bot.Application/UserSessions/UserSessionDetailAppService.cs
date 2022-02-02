using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Quaestor.Bot.Markets;
using Quaestor.Bot.UserSessions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quaestor.Bot.UserSessions
{
    [AbpAuthorize]
    public class UserSessionDetailAppService : BotAppServiceBase, IUserSessionDetailAppService
    {
        private readonly IRepository<UserSessionDetail> _userSessionDetailRepository;
        private readonly IRepository<Market> _marketRepository;

        public UserSessionDetailAppService(IRepository<UserSessionDetail> userSessionDetailRepository, IRepository<Market> marketRepository)
        {
            _userSessionDetailRepository = userSessionDetailRepository;
            _marketRepository = marketRepository;

        }
        private ListResultDto<UserSessionDetaiListDto> GetUserSessionByUserSessionId(UserSessionDetailSearchInput input)
        {
            try
            {
                var sessionDetails = _userSessionDetailRepository
              .GetAll()
              .Where(

                  r => r.UserSessionId == input.UserSessionId
              )
              .ToList();

                return new ListResultDto<UserSessionDetaiListDto>(ObjectMapper.Map<List<UserSessionDetaiListDto>>(sessionDetails));
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        private async Task CreateUserSessionDetail(EditUserSessionDetailInput input)
        {
            try
            {
                var sessionDetail = ObjectMapper.Map<UserSessionDetail>(input);
                await _userSessionDetailRepository.InsertAsync(sessionDetail);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                ;
            }
        }

        public async Task CreateUpdateUserSessionDetail(EditUserSessionDetailInput input)
        {
            try
            {
                if (input.Id > 0)
                {
                    //await EditUserSessionDetail(input, false);

                    #region old
                    //var userSessionMarketsDetails = _userSessionDetailRepository.GetAll().Where(x => x.UserSessionId == input.UserSessionId).ToList();
                    //if (userSessionMarketsDetails != null && userSessionMarketsDetails.Count > 0)
                    //{
                    //    if (input.SupportedCurrency.ToLower() != input.EditedCurrency.ToLower())
                    //    {
                    //        UserSessionDetailSearchInput param = new UserSessionDetailSearchInput();

                    //        int[] marketIds = userSessionMarketsDetails.Select(x => x.MarketId).ToArray();
                    //        int[] supportedMarketIds = _marketRepository.GetAll().Where(x => marketIds.Contains(x.Id) && x.Name.ToLower().EndsWith(input.SupportedCurrency.ToLower())).Select(x => x.Id).ToArray();
                    //        int[] editedMarketIds = _marketRepository.GetAll().Where(x => marketIds.Contains(x.Id) && x.Name.ToLower().EndsWith(input.EditedCurrency.ToLower())).Select(x => x.Id).ToArray();

                    //        //current supporting 
                    //        if (supportedMarketIds.Length == 0)
                    //        {
                    //            var totalBalanceSupporting = userSessionMarketsDetails.Where(x => supportedMarketIds.Contains(x.MarketId)).Select(x => x.BTCAllocated).Sum();
                    //            var allotedBalanceSupporting = totalBalanceSupporting / (supportedMarketIds.Length + 1);
                    //            var SupportedCurrenciesResult = userSessionMarketsDetails.Where(x => supportedMarketIds.Contains(x.MarketId)).ToList();
                    //            foreach (var item in SupportedCurrenciesResult)
                    //            {
                    //                item.BTCAllocated = allotedBalanceSupporting;
                    //                await _userSessionDetailRepository.UpdateAsync(item);
                    //                await CurrentUnitOfWork.SaveChangesAsync();
                    //            }
                    //        }


                    //        if (editedMarketIds.Length == 1)
                    //        {
                    //            var totalBalanceEdited = userSessionMarketsDetails.Where(x => editedMarketIds.Contains(x.MarketId)).Select(x => x.BTCAllocated).Sum();
                    //            var allotedBalanceEdited = totalBalanceEdited / (editedMarketIds.Length - 1);
                    //            var EditedCurrenciesResult = userSessionMarketsDetails.Where(x => editedMarketIds.Contains(x.MarketId)).ToList();
                    //            foreach (var item in EditedCurrenciesResult)
                    //            {
                    //                item.BTCAllocated = allotedBalanceEdited;
                    //                await _userSessionDetailRepository.UpdateAsync(item);
                    //                await CurrentUnitOfWork.SaveChangesAsync();
                    //            }
                    //        }

                    //        //Old edited
                    //        if (editedMarketIds.Length > 1)
                    //        {
                    //            var totalBalanceEdited = userSessionMarketsDetails.Where(x => editedMarketIds.Contains(x.MarketId)).Select(x => x.BTCAllocated).Sum();
                    //            var allotedBalanceEdited = totalBalanceEdited / (editedMarketIds.Length - 1);
                    //            var EditedCurrenciesResult = userSessionMarketsDetails.Where(x => editedMarketIds.Contains(x.MarketId)).ToList();
                    //            foreach (var item in EditedCurrenciesResult)
                    //            {
                    //                item.BTCAllocated = allotedBalanceEdited;
                    //                await _userSessionDetailRepository.UpdateAsync(item);
                    //                await CurrentUnitOfWork.SaveChangesAsync();
                    //            }
                    //        }

                    //       // decimal? BTCAllocatedUpdated = input.CurrentTradingCurrecnyAvailableBalance;
                    //    }
                    //    else
                    //    {
                    //        await EditUserSessionDetail(input, false);
                    //    }
                    //}

                    #endregion

                    
                    if (input.SupportedCurrency.ToLower() != input.EditedCurrency.ToLower())
                    {
                        decimal? BTCAllocatedUpdated = 0;
                        UserSessionDetailSearchInput param = new UserSessionDetailSearchInput();
                        ListResultDto<UserSessionDetaiListDto> result = new ListResultDto<UserSessionDetaiListDto>();
                        ListResultDto<UserSessionDetaiListDto> SupportedCurrenciesResult = new ListResultDto<UserSessionDetaiListDto>();
                        ListResultDto<UserSessionDetaiListDto> EditedCurrenciesResult = new ListResultDto<UserSessionDetaiListDto>();

                        param.UserSessionId = input.UserSessionId;
                        result = GetUserSessionByUserSessionId(param);
                        int[] marketIds = result.Items.Select(x => x.MarketId).ToArray();
                        int[] supportedMarketIds = _marketRepository.GetAll().Where(x => marketIds.Contains(x.Id) && x.Name.ToLower().EndsWith(input.SupportedCurrency.ToLower())).Select(x => x.Id).ToArray();
                        int[] editedMarketIds = _marketRepository.GetAll().Where(x => marketIds.Contains(x.Id) && x.Name.ToLower().EndsWith(input.EditedCurrency.ToLower())).Select(x => x.Id).ToArray();

                         SupportedCurrenciesResult.Items = result.Items.Where(x => supportedMarketIds.Contains(x.MarketId)).ToList();
                         EditedCurrenciesResult.Items = result.Items.Where(x => editedMarketIds.Contains(x.MarketId)).ToList();

                        if (editedMarketIds.Length > 1)
                        {
                            var totalBalanceEdited = result.Items.Where(x => editedMarketIds.Contains(x.MarketId)).Select(x => x.BTCAllocated).Sum();
                            var allotedBalanceEdited = totalBalanceEdited / (editedMarketIds.Length - 1);                            
                            BTCAllocatedUpdated = allotedBalanceEdited;
                            await UpdatePreviousUserSessionDetailItems(EditedCurrenciesResult, BTCAllocatedUpdated);
                        }
                        //current supporting 
                        //if (supportedMarketIds.Length == 0)
                        //{
                        //    input.BTCAllocated = input.CurrentTradingCurrecnyAvailableBalance;
                        //    await EditUserSessionDetail(input, true);
                        //}
                        if (supportedMarketIds.Length > 0)
                        {
                            var totalBalanceSupporting = result.Items.Where(x => supportedMarketIds.Contains(x.MarketId)).Select(x => x.BTCAllocated).Sum();
                            var allotedBalanceSupporting = totalBalanceSupporting / (supportedMarketIds.Length + 1);                            
                            BTCAllocatedUpdated = allotedBalanceSupporting;
                            await UpdatePreviousUserSessionDetailItems(SupportedCurrenciesResult, BTCAllocatedUpdated);                           
                        }

                        input.BTCAllocated = input.CurrentTradingCurrecnyAvailableBalance;
                        await EditUserSessionDetail(input, true);

                        //Edited Markets 
                        //if (editedMarketIds.Length == 1)
                        //{
                        //    input.BTCAllocated = input.CurrentTradingCurrecnyAvailableBalance;
                        //    await EditUserSessionDetail(input, true);
                        //}

                    }
                    else
                    {
                        await EditUserSessionDetail(input, false);
                    }


                }
                else
                {
                    //Get aLl SesiionDEtail against Session DEtail id
                    //Pick firsor default and get allocated btc / (.count+1)
                    // insert new data with calculted btc
                    // and update all prev records

                    UserSessionDetailSearchInput param = new UserSessionDetailSearchInput();
                    ListResultDto<UserSessionDetaiListDto> result = new ListResultDto<UserSessionDetaiListDto>();
                    param.UserSessionId = input.UserSessionId;
                    result = GetUserSessionByUserSessionId(param);
                    int[] marketIds = result.Items.Select(x => x.MarketId).ToArray();
                    int[] supportedMarketIds = _marketRepository.GetAll().Where(x => marketIds.Contains(x.Id) && x.Name.ToLower().EndsWith(input.SupportedCurrency.ToLower())).Select(x => x.Id).ToArray();
                    if (supportedMarketIds != null && supportedMarketIds.Length > 0)
                    {
                        result.Items = result.Items.Where(x => supportedMarketIds.Contains(x.MarketId)).ToList();
                    }
                    else
                    {
                        result = null;
                    }
                    decimal? BTCAllocatedUpdated = input.CurrentTradingCurrecnyAvailableBalance;

                    if (result != null && result.Items.Count > 0)
                    {
                        foreach (var item in result.Items)
                        {
                            EditUserSessionDetailInput ObjEditUserSessionDetailInput = new EditUserSessionDetailInput();

                            ObjEditUserSessionDetailInput.Id = Convert.ToInt32(item.Id);

                            ObjEditUserSessionDetailInput.UserSessionId = item.UserSessionId;
                            ObjEditUserSessionDetailInput.MarketId = item.MarketId;
                            ObjEditUserSessionDetailInput.BTCAllocated = item.BTCAllocated;
                            ObjEditUserSessionDetailInput.TradeProfitPercentage = item.TradeProfitPercentage;
                            ObjEditUserSessionDetailInput.FirstBuyEquityPercentage = item.FirstBuyEquityPercentage;
                            ObjEditUserSessionDetailInput.RebuyPercentage = item.RebuyPercentage;
                            ObjEditUserSessionDetailInput.BuyStopUp = item.BuyStopUp;
                            ObjEditUserSessionDetailInput.BuyStopDown = item.BuyStopDown;
                            ObjEditUserSessionDetailInput.FirstRebuy = item.FirstRebuy;
                            ObjEditUserSessionDetailInput.SecondRebuy = item.SecondRebuy;
                            ObjEditUserSessionDetailInput.ThirdRebuy = item.ThirdRebuy;
                            ObjEditUserSessionDetailInput.FourthRebuy = item.FourthRebuy;
                            ObjEditUserSessionDetailInput.FifthRebuy = item.FifthRebuy;
                            ObjEditUserSessionDetailInput.SixthRebuy = item.SixthRebuy;
                            ObjEditUserSessionDetailInput.SeventRebuy = item.SeventRebuy;
                            ObjEditUserSessionDetailInput.FirstRebuyDrop = item.FirstRebuyDrop;
                            ObjEditUserSessionDetailInput.SecondRebuyDrop = item.SecondRebuyDrop;
                            ObjEditUserSessionDetailInput.ThirdRebuyDrop = item.ThirdRebuyDrop;
                            ObjEditUserSessionDetailInput.FourthRebuyDrop = item.FourthRebuyDrop;
                            ObjEditUserSessionDetailInput.FifthRebuyDrop = item.FifthRebuyDrop;
                            ObjEditUserSessionDetailInput.SixthRebuyDrop = item.SixthRebuyDrop;
                            ObjEditUserSessionDetailInput.SeventhRebuyDrop = item.SeventhRebuyDrop;
                            ObjEditUserSessionDetailInput.ExchangeId = item.ExchangeId;

                            ObjEditUserSessionDetailInput.BTCAllocated = BTCAllocatedUpdated;

                            await EditUserSessionDetail(ObjEditUserSessionDetailInput, true);
                        }
                    }
                    input.BTCAllocated = BTCAllocatedUpdated;
                    await CreateUserSessionDetail(input);

                }
            }
            catch (Exception ex)
            {
                await Task.FromResult(0);
            }

        }

        private async Task EditUserSessionDetail(EditUserSessionDetailInput input, bool IsBalanceUpdate)
        {
            try
            {
                var userSessionDetail = await _userSessionDetailRepository.GetAsync(input.Id);
                userSessionDetail.UserSessionId = input.UserSessionId;
                userSessionDetail.MarketId = input.MarketId;
                if (IsBalanceUpdate)
                    userSessionDetail.BTCAllocated = input.BTCAllocated;
                userSessionDetail.TradeProfitPercentage = input.TradeProfitPercentage;
                userSessionDetail.FirstBuyEquityPercentage = input.FirstBuyEquityPercentage;
                userSessionDetail.RebuyPercentage = input.RebuyPercentage;
                userSessionDetail.BuyStopUp = input.BuyStopUp;
                userSessionDetail.BuyStopDown = input.BuyStopDown;
                userSessionDetail.FirstRebuy = input.FirstRebuy;
                userSessionDetail.SecondRebuy = input.SecondRebuy;
                userSessionDetail.ThirdRebuy = input.ThirdRebuy;
                userSessionDetail.FourthRebuy = input.FourthRebuy;
                userSessionDetail.FifthRebuy = input.FifthRebuy;
                userSessionDetail.SixthRebuy = input.SixthRebuy;
                userSessionDetail.SeventRebuy = input.SeventRebuy;
                userSessionDetail.FirstRebuyDrop = input.FirstRebuyDrop;
                userSessionDetail.SecondRebuyDrop = input.SecondRebuyDrop;
                userSessionDetail.ThirdRebuyDrop = input.ThirdRebuyDrop;
                userSessionDetail.FourthRebuyDrop = input.FourthRebuyDrop;
                userSessionDetail.FifthRebuyDrop = input.FifthRebuyDrop;
                userSessionDetail.SixthRebuyDrop = input.SixthRebuyDrop;
                userSessionDetail.SeventhRebuyDrop = input.SeventhRebuyDrop;
                userSessionDetail.ExchangeId = input.ExchangeId;
                await _userSessionDetailRepository.UpdateAsync(userSessionDetail);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await Task.FromResult(0);
            }
        }

        public async Task<UserSessionDetailDto> GetUserSessionDetailById(SearchUserSessionDetailSearch input)
        {
            try
            {
                var userKey = await _userSessionDetailRepository.GetAsync(input.Id);
                UserSessionDetailDto sessionDetail = new UserSessionDetailDto();
                sessionDetail.UserSessionId = userKey.UserSessionId;
                sessionDetail.MarketId = userKey.MarketId;
                sessionDetail.BTCAllocated = userKey.BTCAllocated;
                sessionDetail.TradeProfitPercentage = userKey.TradeProfitPercentage;
                sessionDetail.FirstBuyEquityPercentage = userKey.FirstBuyEquityPercentage;
                sessionDetail.RebuyPercentage = userKey.RebuyPercentage;
                sessionDetail.BuyStopUp = userKey.BuyStopUp;
                sessionDetail.BuyStopDown = userKey.BuyStopDown;
                sessionDetail.FirstRebuy = userKey.FirstRebuy;
                sessionDetail.SecondRebuy = userKey.SecondRebuy;
                sessionDetail.ThirdRebuy = userKey.ThirdRebuy;
                sessionDetail.FourthRebuy = userKey.FourthRebuy;
                sessionDetail.FifthRebuy = userKey.FifthRebuy;
                sessionDetail.SixthRebuy = userKey.SixthRebuy;
                sessionDetail.SeventRebuy = userKey.SeventRebuy;
                sessionDetail.FirstRebuyDrop = userKey.FirstRebuyDrop;
                sessionDetail.SecondRebuyDrop = userKey.SecondRebuyDrop;
                sessionDetail.ThirdRebuyDrop = userKey.ThirdRebuyDrop;
                sessionDetail.FourthRebuyDrop = userKey.FourthRebuyDrop;
                sessionDetail.FifthRebuyDrop = userKey.FifthRebuyDrop;
                sessionDetail.SixthRebuyDrop = userKey.SixthRebuyDrop;
                sessionDetail.SeventhRebuyDrop = userKey.SeventhRebuyDrop;
                sessionDetail.ExchangeId = userKey.ExchangeId;
                sessionDetail.Id = userKey.Id;
                return sessionDetail;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public async Task UpdatePreviousUserSessionDetailItems(ListResultDto<UserSessionDetaiListDto> result, decimal? BTCAllocatedUpdated)
        {
            try
            {
                if (result != null && result.Items.Count > 0)
                {
                    foreach (var item in result.Items)
                    {
                        EditUserSessionDetailInput ObjEditUserSessionDetailInput = new EditUserSessionDetailInput();

                        ObjEditUserSessionDetailInput.Id = Convert.ToInt32(item.Id);

                        ObjEditUserSessionDetailInput.UserSessionId = item.UserSessionId;
                        ObjEditUserSessionDetailInput.MarketId = item.MarketId;
                        ObjEditUserSessionDetailInput.BTCAllocated = item.BTCAllocated;
                        ObjEditUserSessionDetailInput.TradeProfitPercentage = item.TradeProfitPercentage;
                        ObjEditUserSessionDetailInput.FirstBuyEquityPercentage = item.FirstBuyEquityPercentage;
                        ObjEditUserSessionDetailInput.RebuyPercentage = item.RebuyPercentage;
                        ObjEditUserSessionDetailInput.BuyStopUp = item.BuyStopUp;
                        ObjEditUserSessionDetailInput.BuyStopDown = item.BuyStopDown;
                        ObjEditUserSessionDetailInput.FirstRebuy = item.FirstRebuy;
                        ObjEditUserSessionDetailInput.SecondRebuy = item.SecondRebuy;
                        ObjEditUserSessionDetailInput.ThirdRebuy = item.ThirdRebuy;
                        ObjEditUserSessionDetailInput.FourthRebuy = item.FourthRebuy;
                        ObjEditUserSessionDetailInput.FifthRebuy = item.FifthRebuy;
                        ObjEditUserSessionDetailInput.SixthRebuy = item.SixthRebuy;
                        ObjEditUserSessionDetailInput.SeventRebuy = item.SeventRebuy;
                        ObjEditUserSessionDetailInput.FirstRebuyDrop = item.FirstRebuyDrop;
                        ObjEditUserSessionDetailInput.SecondRebuyDrop = item.SecondRebuyDrop;
                        ObjEditUserSessionDetailInput.ThirdRebuyDrop = item.ThirdRebuyDrop;
                        ObjEditUserSessionDetailInput.FourthRebuyDrop = item.FourthRebuyDrop;
                        ObjEditUserSessionDetailInput.FifthRebuyDrop = item.FifthRebuyDrop;
                        ObjEditUserSessionDetailInput.SixthRebuyDrop = item.SixthRebuyDrop;
                        ObjEditUserSessionDetailInput.SeventhRebuyDrop = item.SeventhRebuyDrop;
                        ObjEditUserSessionDetailInput.ExchangeId = item.ExchangeId;

                        ObjEditUserSessionDetailInput.BTCAllocated = BTCAllocatedUpdated;

                        await EditUserSessionDetail(ObjEditUserSessionDetailInput, true);
                    }
                }

            }
            catch (Exception)
            {

            }

        }

    }
}
