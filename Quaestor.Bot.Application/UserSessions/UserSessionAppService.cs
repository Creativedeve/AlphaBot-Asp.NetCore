using Abp.Application.Services;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using BinanceExchange.API.Client;
using Microsoft.AspNetCore.Identity;
using Quaestor.Bot.DashBoard;
using Quaestor.Bot.ErrorLogs;
using Quaestor.Bot.JobManagement.Jobs.JobImplementation;
using Quaestor.Bot.JobManagement.StateHandler;
using Quaestor.Bot.Markets;
using Quaestor.Bot.PurchaseOrderRecords;
using Quaestor.Bot.UserKeys;
using Quaestor.Bot.UserKeys.Dto;
using Quaestor.Bot.UserSessions.Dto;
using Quaestor.Bot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeInfo = Quaestor.Bot.ExchangesInfo.ExchangeInfo;

namespace Quaestor.Bot.UserSessions
{
    [AbpAuthorize]
    public class UserSessionAppService : AsyncCrudAppService<UserSession, UserSessionDto, int, UserSessionSearchInput, CreateUserSessionInput, EditUserSessionInput, UserSessionSearchInput, DeleteInput>, IUserSessionAppService
    {
        #region Properties
        private readonly IRepository<UserSession, int> _userSessionRepository;
        private readonly IRepository<Market, int> _marketRepository;
        private readonly IRepository<UserKey, int> _userKeyRepository;
        private readonly IRepository<PurchaseOrderRecord, int> _purchaseOrderRepository;
        private readonly IRepository<ErrorLog, int> _errorLogTableRepository;
        private ExchangeInfo _exchangeInfo;
        private readonly ErrorLogAppService errorLogAppService;
        private readonly ErrorLogAppService _errorLogRepository;

        private readonly IUserSessionRepository _userSessionEntityFrameworkRepository;
        private readonly IRepository<UserSessionDetail, int> _userSessionDetailRepository;
        private ISaleImmediatelySynchronization _saleImmediatelySynchronization;
        private CacheManagement.Redis.ICacheProvider _cacheProvider;
        private readonly IRepository<PreserveBuyInformation.PreserveBuyInfo, int> _preserveBuyInfoRepository;
        #endregion

        #region Constructor
        public UserSessionAppService(IRepository<UserSession, int> repository, IRepository<Market, int> marketRepository, IRepository<UserKey, int> userKeyRepository, IRepository<PurchaseOrderRecord, int> purchaseOrderRepositor, IRepository<ErrorLog> errorLogRepositor, IRepository<ErrorLog> errorLogTableRepository, IUserSessionRepository userSessionEntityFrameworkRepository, IRepository<UserSessionDetail, int> userSessionDetailRepository, IRepository<PreserveBuyInformation.PreserveBuyInfo, int> preserveBuyInfoRepository)
          : base(repository)
        {
            _userSessionRepository = repository;
            _marketRepository = marketRepository;
            _exchangeInfo = new ExchangeInfo();
            _userKeyRepository = userKeyRepository;
            _purchaseOrderRepository = purchaseOrderRepositor;
            _errorLogRepository = new ErrorLogAppService(errorLogRepositor);
            _errorLogTableRepository = errorLogTableRepository;

            _userSessionEntityFrameworkRepository = userSessionEntityFrameworkRepository;
            _userSessionDetailRepository = userSessionDetailRepository;
            _cacheProvider = new CacheManagement.Redis.RedisCacheProvider();
            _preserveBuyInfoRepository = preserveBuyInfoRepository;

        }
        #endregion

        #region Methods
        public async Task CreateUserSession(CreateUserSessionInput input)
        {
            try
            {
                CreateUserSessionDetailInput userSessionDetail = new CreateUserSessionDetailInput();
                foreach (var item in input.UserSessionDetails)
                {

                    userSessionDetail.UserSessionId = item.UserSessionId;
                    userSessionDetail.MarketId = item.MarketId;
                    userSessionDetail.BTCAllocated = item.BTCAllocated;
                    userSessionDetail.TradeProfitPercentage = item.TradeProfitPercentage;
                    userSessionDetail.FirstBuyEquityPercentage = item.FirstBuyEquityPercentage;
                    userSessionDetail.RebuyPercentage = item.RebuyPercentage;
                    userSessionDetail.BuyStopUp = item.BuyStopUp;
                    userSessionDetail.BuyStopDown = item.BuyStopDown;
                    userSessionDetail.FirstRebuy = item.FirstRebuy;
                    userSessionDetail.SecondRebuy = item.SecondRebuy;
                    userSessionDetail.ThirdRebuy = item.ThirdRebuy;
                    userSessionDetail.FourthRebuy = item.FourthRebuy;
                    userSessionDetail.FifthRebuy = item.FifthRebuy;
                    userSessionDetail.SixthRebuy = item.SixthRebuy;
                    userSessionDetail.SeventRebuy = item.SeventRebuy;
                    userSessionDetail.FirstRebuyDrop = item.FirstRebuyDrop;
                    userSessionDetail.SecondRebuyDrop = item.SecondRebuyDrop;
                    userSessionDetail.ThirdRebuyDrop = item.ThirdRebuyDrop;
                    userSessionDetail.FourthRebuyDrop = item.FourthRebuyDrop;
                    userSessionDetail.FifthRebuyDrop = item.FifthRebuyDrop;
                    userSessionDetail.SixthRebuyDrop = item.SixthRebuyDrop;
                    userSessionDetail.SeventhRebuyDrop = item.SeventhRebuyDrop;
                    userSessionDetail.ExchangeId = item.ExchangeId;

                }
                input.UserSessionDetails.Clear();
                input.UserSessionDetails.Add(userSessionDetail);
                var userSession = ObjectMapper.Map<UserSession>(input);
                userSession.IsSessionClose = false;
                userSession.IsSessionInProgress = true;

                await _userSessionRepository.InsertAsync(userSession);
                await CurrentUnitOfWork.SaveChangesAsync();

            }
            catch (System.Exception ex)
            {

                
            }

        }
        public UserSessionDto GetUserSessionByUserId(UserSessionSearchInput input)
        {
            try
            {
                var userSession = Repository.GetAllIncluding(p => p.UserSessionDetails).OrderByDescending(x => x.Id).FirstOrDefault(p => p.UserId == input.UserId && p.IsActive == false && p.IsSessionClose == false && p.IsSessionInProgress == true);
                if (userSession == null)
                {
                    return null;
                }
                return MapToEntityDto(userSession);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public UserSessionDto GetActiveUserSessionByUserId(UserSessionSearchInput input)
        {
            try
            {
                var userSession = Repository.GetAllIncluding(p => p.UserSessionDetails).FirstOrDefault(p => p.UserId == input.UserId && p.IsActive == true);
                if (userSession == null)
                {
                    return null;
                }
                return MapToEntityDto(userSession);
            }
            catch (Exception)
            {
                return null;
            }

        }

        public UserSessionDto GetUserSessionByUserSessionId(UserSessionSearchInput input)
        {

            var userSession = Repository.GetAllIncluding(p => p.UserSessionDetails).FirstOrDefault(p => p.Id == input.Id && p.IsActive == true);
            if (userSession == null)
            {
                //throw new EntityNotFoundException("Entity Not Found");
                return null;
            }
            return MapToEntityDto(userSession);
        }
        public async Task<string> StartUserSession(UserSessionStartInput input)
        {
            try
            {

                string ResponseString = "";
                UserSession userSessionResult = new UserSession();

                var userSession = _userSessionRepository.GetAllIncluding(u => u.UserSessionDetails).FirstOrDefault(p => p.UserId == input.UserId && p.Id == input.Id);
                userSession.IsActive = true;
                userSessionResult = await _userSessionRepository.UpdateAsync(userSession);
                CurrentUnitOfWork.SaveChanges();
                if (userSessionResult != null)
                {
                    bool OrderPlaced = await IocManager.Instance.Resolve<FirstBuyManagement>().ExecuteUserOrder(Convert.ToInt32(userSessionResult.UserId));

                    ResponseString = "Session is Started";

                }

                return ResponseString;

            }
            catch (Exception ex)
            {
                return "";
            }

        }
        [Abp.Domain.Uow.UnitOfWork]
        public async Task<string> CloseMarketPosition(UserSessionClosePosition input)
        {
            try
            {
                string responseResult = "";
                UserSessionDetail userSessionDetailResult = null;
                _saleImmediatelySynchronization = new SaleImmediatelySynchronization();
                var userSession = _userSessionRepository.GetAllIncluding(u => u.UserSessionDetails).FirstOrDefault(p => p.UserId == input.UserId && p.Id == input.Id && p.IsActive == true);

                if (userSession != null)
                {
                    if (userSession.UserSessionDetails != null && userSession.UserSessionDetails.Count > 0)
                    {

                        var UserSessionDetail = userSession.UserSessionDetails.Where(x => x.CreatorUserId == input.UserId && x.UserSessionId == userSession.Id && x.MarketId == input.MarketId && x.IsSessionClosed != true).FirstOrDefault();
                        var purchaseOrderRecord = _purchaseOrderRepository.GetAllList().Where(p => p.UserSessionDetailId == UserSessionDetail.Id && p.MarketId == UserSessionDetail.MarketId && p.IsSold != true).FirstOrDefault();

                        if (input.IsSaleImmediately)
                        {
                            if (purchaseOrderRecord != null)
                            {
                                var immediatelySellDetail = _userSessionEntityFrameworkRepository.SellAllBoughtRecordsAgainstUser(Convert.ToInt64(purchaseOrderRecord.CreatorUserId), purchaseOrderRecord.Id, purchaseOrderRecord.MarketId);
                                if (immediatelySellDetail != null)
                                {
                                    await _saleImmediatelySynchronization.SendSaleImmediatelyDataInQueue(immediatelySellDetail);
                                }                                
                            }
                        }
                        UserSessionDetail.IsSessionClosed = true;
                        userSessionDetailResult = await _userSessionDetailRepository.UpdateAsync(UserSessionDetail);
                        await CurrentUnitOfWork.SaveChangesAsync();
                        if (IsAllMarketClosed(UserSessionDetail.UserSessionId).Result)
                        {
                            
                            if (userSession != null)
                            {
                                userSession.IsActive = false;
                                userSession.IsSessionClose = true;
                                userSession.IsSessionInProgress = false;
                                userSession.End = DateTime.UtcNow;
                                await _userSessionRepository.UpdateAsync(userSession);                              
                            }
                        }
                        if (userSessionDetailResult != null)
                        {
                            responseResult = "Success";
                        }
                    }
                }
                return responseResult;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public async Task<bool> IsAllMarketClosed(int UserSessionId)
        {
            bool isAllMarketClosed = false;
            var marketsAgainstSession = await _userSessionDetailRepository.GetAllListAsync(s=>s.UserSessionId==UserSessionId && s.IsDeleted!=true&&s.IsSessionClosed!=true);
            if (marketsAgainstSession.Count == 0)
                 isAllMarketClosed = true;
            return isAllMarketClosed;
        }
        public async Task<List<UserSession>> GetAllUserSessionsByUserId(UserSessionSearchInput input)
        {
            try
            {
                var lstUserSession = await Repository.GetAllListAsync(p => p.UserId == input.UserId);
                if (lstUserSession == null)
                {
                    return null;
                }
                return lstUserSession;
            }
            catch (Exception)
            {
                return null;
            }

        }
        [Abp.Domain.Uow.UnitOfWork]
        public async Task<string> CloseActiveSessionPosition(UserSessionClosePosition input)
        {
            try
            {
                _saleImmediatelySynchronization = new SaleImmediatelySynchronization();
                string responseResult = "";
                UserSession userSessionResult = null;
                UserSessionDetail userSessionDetailResult = null;

                var userSession = _userSessionRepository.GetAllIncluding(u => u.UserSessionDetails).FirstOrDefault(p => p.UserId == input.UserId && p.Id == input.Id && p.IsActive == true);
                if (userSession != null)
                {
                    if (userSession.UserSessionDetails != null && userSession.UserSessionDetails.Count > 0)
                    {
                        foreach (var UserSessionDetail in userSession.UserSessionDetails)
                        {
                            if (input.IsSaleImmediately && !UserSessionDetail.IsSessionClosed)
                            {
                                var purchaseOrderRecord = _purchaseOrderRepository.GetAllList().Where(p => p.UserSessionDetailId == UserSessionDetail.Id && p.MarketId == UserSessionDetail.MarketId && p.IsSold != true).FirstOrDefault();
                                if (purchaseOrderRecord != null)
                                {
                                    var immediatelySellDetail = _userSessionEntityFrameworkRepository.SellAllBoughtRecordsAgainstUser(Convert.ToInt64(purchaseOrderRecord.CreatorUserId), purchaseOrderRecord.Id, purchaseOrderRecord.MarketId);
                                    if (immediatelySellDetail != null)
                                    {
                                        await _saleImmediatelySynchronization.SendSaleImmediatelyDataInQueue(immediatelySellDetail);
                                    }
                                }
                            }
                            UserSessionDetail.IsSessionClosed = true;
                            UserSessionDetail.LastModificationTime = DateTime.UtcNow;
                            UserSessionDetail.LastModifierUserId = UserSessionDetail.CreatorUserId;
                            userSessionDetailResult = await _userSessionDetailRepository.UpdateAsync(UserSessionDetail);
                        }
                    }
                    userSession.IsActive = false;
                    userSession.IsSessionClose = true;
                    userSession.IsSessionInProgress = false;
                    userSession.End = DateTime.UtcNow;
                    userSessionResult = await _userSessionRepository.UpdateAsync(userSession);

                    if (userSessionResult != null)
                    {
                        responseResult = "Success";
                    }
                }

                return responseResult;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public async Task<DashboardDetail> GetDashboardDetail(UserPurchaseOrderInput input)
        {

            try
            {
                DashboardDetail dashboardDetail = new DashboardDetail();
                dashboardDetail.DashboardResults = await GetUserDashboardDetail(input);
                dashboardDetail.ProfitLossStatisticsDetails = await GetUserProfitLossStatisticsGraph(input);
                UserKeySearchByUserIdInput userKeySearchByUserIdInput = new UserKeySearchByUserIdInput();
                userKeySearchByUserIdInput.UserId = input.UserId ?? 0;
                var userKeyAppService = IocManager.Instance.Resolve<UserKeyAppService>();
                dashboardDetail.BinanceBalances = await userKeyAppService.GetUserBalanceDetail(userKeySearchByUserIdInput);
                dashboardDetail.BalanceInfo = await userKeyAppService.GetUserBalance(userKeySearchByUserIdInput);

                return dashboardDetail;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<bool> DeleteUserMarket(DeleteInput deleteInput)
        {
            bool iSRecordDeleted = false;
            var userSessionDetailInfo = await _userSessionDetailRepository.GetAsync(deleteInput.UserSessionDetailId);           
            if (userSessionDetailInfo != null)
            {
                try
                {
                    var userSessionMarketsDetails = _userSessionDetailRepository.GetAll().Where(x => x.UserSessionId == userSessionDetailInfo.UserSessionId).ToList();
                    if (userSessionMarketsDetails.Count == 1)
                    {
                        await _userSessionDetailRepository.HardDeleteAsync(userSessionDetailInfo);
                        await CurrentUnitOfWork.SaveChangesAsync();
                        iSRecordDeleted = true;
                    }
                    else
                    {
                        

                        int[] marketIds = userSessionMarketsDetails.Select(x => x.MarketId).ToArray();
                        var CurrentCurrencies = _marketRepository.Get(userSessionDetailInfo.MarketId).Name.Split("/")[1];                       
                        int[] supportedMarketIds = _marketRepository.GetAll().Where(x => marketIds.Contains(x.Id) && x.Name.ToLower().EndsWith(CurrentCurrencies.ToLower())).Select(x => x.Id).ToArray();
                        if (supportedMarketIds != null && supportedMarketIds.Length == 1)
                        {
                            await _userSessionDetailRepository.HardDeleteAsync(userSessionDetailInfo);
                            await CurrentUnitOfWork.SaveChangesAsync();
                            iSRecordDeleted = true;
                            //userSessionMarketsDetails = userSessionMarketsDetails.Where(x => supportedMarketIds.Contains(x.MarketId)).ToList();
                        }
                        else if(supportedMarketIds != null && supportedMarketIds.Length > 1)
                        {
                            var totalBalance = userSessionMarketsDetails.Where(x => supportedMarketIds.Contains(x.MarketId)).Select(x => x.BTCAllocated).Sum();
                            var allotedBTC = totalBalance / (supportedMarketIds.Length-1);


                            userSessionMarketsDetails = userSessionMarketsDetails.Where(x => supportedMarketIds.Contains(x.MarketId)).ToList();
                            foreach (var item in userSessionMarketsDetails)
                            {
                                item.BTCAllocated = allotedBTC;
                                await _userSessionDetailRepository.UpdateAsync(item);
                                await CurrentUnitOfWork.SaveChangesAsync();
                            }
                            await _userSessionDetailRepository.HardDeleteAsync(userSessionDetailInfo);
                            await CurrentUnitOfWork.SaveChangesAsync();
                            iSRecordDeleted = true;
                        }
                    }                 
                }
                catch (Exception)
                {
                    return iSRecordDeleted;
                }
             
            }
            return iSRecordDeleted;
        }
        #endregion

        #region Helper Methods
        protected override UserSessionDto MapToEntityDto(UserSession userSession)
        {
            try
            {
                var userSessionDto = base.MapToEntityDto(userSession);
                userSessionDto.UserSessionDetails = userSession.UserSessionDetails.ToList();
                return userSessionDto;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        private void CheckErrors(IdentityResult identityResult)
        {
            try
            {
                identityResult.CheckErrors(LocalizationManager);
            }
            catch (Exception ex)
            {
                
            }

        }

        public async Task<List<Binance.Net.Objects.BinanceOrder>> GetUserPurchaseOrder(UserPurchaseOrderInput input)
        {
            try
            {
                var userKey = _userKeyRepository.GetAll().Where(u => u.UserId == input.UserId).FirstOrDefault();

                Binance.Net.BinanceClient lObj_BinanceClient;

                lObj_BinanceClient = new Binance.Net.BinanceClient();
                lObj_BinanceClient.SetApiCredentials(userKey.ApiKey, userKey.SecretKey);


                List<Binance.Net.Objects.BinanceOrder> lstBinanceOrders = null;

                List<PurchaseOrderRecord> lstPurchaseOrderRecord = await _purchaseOrderRepository.GetAllListAsync(p => p.CreatorUserId == input.UserId);
                if (lstPurchaseOrderRecord != null && lstPurchaseOrderRecord.Count > 0)
                {
                    lstBinanceOrders = new List<Binance.Net.Objects.BinanceOrder>();

                    foreach (PurchaseOrderRecord item in lstPurchaseOrderRecord)
                    {
                        var result = await lObj_BinanceClient.GetAllOrdersAsync(item.MarketName, item.OrderId);

                        if (result != null)
                        {
                            for (int i = 0; i < result.Data.Length; i++)
                            {
                                lstBinanceOrders.Add(result.Data[i]);
                            }
                        }
                    }
                    //throw new EntityNotFoundException("Entity Not Found");
                    // return null;
                }
                return lstBinanceOrders;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<List<DashboardResult>> GetUserDashboardDetail(UserPurchaseOrderInput input)
        {
            try
            {
                List<DashboardResult> lstDashboardResultDto = new List<DashboardResult>();
                lstDashboardResultDto = await _userSessionEntityFrameworkRepository.GetUserDashboardDetail(input.UserId);
                if (lstDashboardResultDto != null)
                {
                    List<CacheManagement.Redis.TickerCache> tickerCacheList = new List<CacheManagement.Redis.TickerCache>();
                    tickerCacheList = _cacheProvider.Get<List<CacheManagement.Redis.TickerCache>>(Common.BuyCacheType.TickersCache.ToString());

                    if (tickerCacheList != null && tickerCacheList.Count > 0)
                    {
                        foreach (var item in lstDashboardResultDto)
                        {
                            var tickerBtc = tickerCacheList.Where(m => m.Key == item.MarketName.Replace("/", "")).FirstOrDefault();
                            if(tickerBtc!=null)
                            item.TickerValue = tickerBtc.Value;
                        }
                    }
                    else
                    {
                        foreach (var item in lstDashboardResultDto)
                        {
                            string tickerBtc = await GetTickerValue(input.UserId, item.MarketName.Replace("/", ""));
                            if (tickerBtc != null)
                                item.TickerValue = Convert.ToDecimal(tickerBtc);
                        }
                    }

                    return lstDashboardResultDto;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<ProfitLossStatisticsDetail>> GetUserProfitLossStatisticsGraph(UserPurchaseOrderInput input)
        {
            try
            {


                List<ProfitLossStatisticsDetail> profitLossStatisticsDetails = new List<ProfitLossStatisticsDetail>();
                profitLossStatisticsDetails = await _userSessionEntityFrameworkRepository.GetUserProfitLossStatisticsGraph(input.UserId);
                if (profitLossStatisticsDetails != null && profitLossStatisticsDetails.Count > 0)
                {

                    List<CacheManagement.Redis.TickerCache> tickerCacheList = new List<CacheManagement.Redis.TickerCache>();
                    tickerCacheList = _cacheProvider.Get<List<CacheManagement.Redis.TickerCache>>(Common.BuyCacheType.TickersCache.ToString());

                    if (tickerCacheList != null && tickerCacheList.Count > 0)
                    {
                        foreach (var item in profitLossStatisticsDetails)
                        {
                            if (item.IsAlreadySold)
                            {
                                var previousBTCValueAfterTP = item.TotalBTCAfterSell;
                                item.MarketName = item.MarketName.Replace("/", "");
                                var tickerBtc = tickerCacheList.Where(m => m.Key == item.MarketName).FirstOrDefault();
                                if (tickerBtc != null)
                                {
                                    var sellRate = tickerBtc.Value;
                                    var BTCValueAfterTP = sellRate * item.TotalBoughtQuantity;

                                    var BTCTradeProfit = BTCValueAfterTP - item.SumBuyWith;

                                    var TotalBTCAfterSell = previousBTCValueAfterTP + BTCTradeProfit;
                                    item.MarketPercentage = (((TotalBTCAfterSell - item.BTCAllocated) / item.BTCAllocated) * 100);
                                }
                            }
                            else if (!item.IsSold)
                            {
                                item.MarketName = item.MarketName.Replace("/", "");
                                var tickerBtc = tickerCacheList.Where(m => m.Key == item.MarketName).FirstOrDefault();
                                if (tickerBtc != null)
                                {
                                    var sellRate = tickerBtc.Value;
                                    var BTCValueAfterTP = sellRate * item.TotalBoughtQuantity;
                                    var BTCTradeProfit = BTCValueAfterTP - item.SumBuyWith;

                                    var TotalBTCAfterSell = BTCTradeProfit + item.BTCAllocated;
                                    item.MarketPercentage = (((TotalBTCAfterSell - item.BTCAllocated) / item.BTCAllocated) * 100);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in profitLossStatisticsDetails)
                        {
                            item.MarketName = item.MarketName.Replace("/", "");
                            if (item.IsAlreadySold)
                            {
                                var previousBTCValueAfterTP = item.TotalBTCAfterSell;                                
                                //var tickerBtc = tickerCacheList.Where(m => m.Key == item.MarketName).FirstOrDefault();
                                //var sellRate = tickerBtc.Value;

                                var tickerBtc = await GetTickerValue(input.UserId, item.MarketName);
                                if (tickerBtc != null)
                                {
                                    var sellRate = Convert.ToDecimal(tickerBtc);

                                    var BTCValueAfterTP = sellRate * item.TotalBoughtQuantity;
                                    var BTCTradeProfit = BTCValueAfterTP - item.SumBuyWith;
                                    var TotalBTCAfterSell = previousBTCValueAfterTP + BTCTradeProfit;
                                    item.MarketPercentage = (((TotalBTCAfterSell - item.BTCAllocated) / item.BTCAllocated) * 100);
                                }
                            }
                            else if (!item.IsSold)
                            {                                
                                //var tickerBtc = tickerCacheList.Where(m => m.Key == item.MarketName).FirstOrDefault();
                                //var sellRate = tickerBtc.Value;

                                var tickerBtc = await GetTickerValue(input.UserId, item.MarketName);
                                if (tickerBtc != null)
                                {
                                    var sellRate = Convert.ToDecimal(tickerBtc);

                                    var BTCValueAfterTP = sellRate * item.TotalBoughtQuantity;
                                    var BTCTradeProfit = BTCValueAfterTP - item.SumBuyWith;
                                    var TotalBTCAfterSell = BTCTradeProfit + item.BTCAllocated;
                                    item.MarketPercentage = (((TotalBTCAfterSell - item.BTCAllocated) / item.BTCAllocated) * 100);
                                }
                            }
                        }
                    }
                    
                    
                    return profitLossStatisticsDetails;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<string> GetTickerValue(int? UserId, string Pair)
        {
            try
            {

                var userKey = _userKeyRepository
               .GetAll()
               .Where(
                   r => r.UserId == UserId
               )
               .FirstOrDefault();
                var client = new BinanceClient(new ClientConfiguration()
                {
                    ApiKey = userKey.ApiKey,
                    SecretKey = userKey.SecretKey,
                });

                Binance.Net.BinanceClient lObj_BinanceClient;
                lObj_BinanceClient = new Binance.Net.BinanceClient();
                lObj_BinanceClient.SetApiCredentials(userKey.ApiKey, userKey.SecretKey);

                var tickerBtc = await lObj_BinanceClient.GetPriceAsync(Pair);
                return Convert.ToString(tickerBtc.Data.Price);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<DashboardResult>> GetUserDashboardBuyUpDownDetail(UserPurchaseOrderInput input)
        {
            try
            {
                List<DashboardResult> lstDashboardResultDto = new List<DashboardResult>();
                lstDashboardResultDto = await _userSessionEntityFrameworkRepository.GetUserDashboardBuyUpDownDetail(Convert.ToInt32(input.UserId));
                if (lstDashboardResultDto != null)
                {
                    List<CacheManagement.Redis.TickerCache> tickerCacheList = new List<CacheManagement.Redis.TickerCache>();
                    tickerCacheList = _cacheProvider.Get<List<CacheManagement.Redis.TickerCache>>(Common.BuyCacheType.TickersCache.ToString());

                    if (tickerCacheList != null && tickerCacheList.Count > 0)
                    {
                        foreach (var item in lstDashboardResultDto)
                        {
                            var tickerBtc = tickerCacheList.Where(m => m.Key == item.MarketName.Replace("/", "")).FirstOrDefault();
                            item.TickerValue = tickerBtc.Value;
                        }
                    }
                    else
                    {
                        foreach (var item in lstDashboardResultDto)
                        {
                            string tickerBtc = await GetTickerValue(input.UserId, item.MarketName.Replace("/", ""));
                            item.TickerValue = Convert.ToDecimal(tickerBtc);
                        }
                    }

                    return lstDashboardResultDto;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }
        #endregion
    }
}
