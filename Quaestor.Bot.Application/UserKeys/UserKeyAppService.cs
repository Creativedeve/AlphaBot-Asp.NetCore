using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BinanceExchange.API.Client;
using BinanceExchange.API.Models.Response;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Identity;
using Quaestor.Bot.Authorization.Users;
using Quaestor.Bot.Exchanges.SupportedTradingMarket;
using Quaestor.Bot.ExchangesInfo;
using Quaestor.Bot.Markets;
using Quaestor.Bot.Products;
using Quaestor.Bot.TradingRules;
using Quaestor.Bot.UserKeys.Dto;
using Quaestor.MLM.Data.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quaestor.Bot.UserKeys
{
    [AbpAuthorize]
    public class UserKeyAppService : BotAppServiceBase, IUserKeyAppService
    {
        #region Properties
        private readonly IRepository<UserKey> _userKeyRepository;
        private readonly ExchangeInfo _exchangeInfo;
        private CacheManagement.Redis.ICacheProvider _cacheProvider;
        private readonly IRepository<TradingParameter> _tradingParameterRepository;
        private readonly IRepository<TradingCurrencies> _tradingCurrenciesRepository;
        private readonly IRepository<Market> _marketRepository;
        private readonly IUserProductsPaymentRecordsDomainService _userProductsPaymentRecordsDomainService;
        private readonly IProductAppService _productAppService;
      
        public ILogger Logger { get; set; }
        private readonly UserManager _userManager; 
        #endregion

        #region Constructor
        public UserKeyAppService(IRepository<UserKey> userKeyRepository, IRepository<TradingParameter> tradingParameterRepository, IRepository<TradingCurrencies> tradingCurrenciesRepository, IRepository<Market> marketRepository, UserManager userManager, IUserProductsPaymentRecordsDomainService userProductsPaymentRecordsDomainService, IProductAppService productAppService)
        {
            _userKeyRepository = userKeyRepository;
            _exchangeInfo = new ExchangeInfo();

            _tradingParameterRepository = tradingParameterRepository;
            _tradingCurrenciesRepository = tradingCurrenciesRepository;
            _cacheProvider = new CacheManagement.Redis.RedisCacheProvider();
            _marketRepository = marketRepository;
            Logger = NullLogger.Instance;
            _userManager = userManager;
            _userProductsPaymentRecordsDomainService = userProductsPaymentRecordsDomainService;
            _productAppService = productAppService;
        }
        #endregion

        #region Methods
        public ListResultDto<UserKeyListDto> GetUserKeyByUserId(UserKeySearchByUserIdInput input)
        {
            try
            {
                var userKey = _userKeyRepository
              .GetAll()
              .Where(

                  r => r.UserId == input.UserId
              )
              .ToList();

                return new ListResultDto<UserKeyListDto>(ObjectMapper.Map<List<UserKeyListDto>>(userKey));
            }
            catch (Exception)
            {
                return null;
            }

        }
        public UserKeyDto GetUserKeyById(UserKeySearchInput input)
        {
            try
            {
                var result = _userKeyRepository
               .GetAll()
               .Where(

                   r => r.Id == input.Id
               ).FirstOrDefault();

                UserKeyDto userKey = new UserKeyDto
                {
                    Id = result.Id,
                    SecretKey = result.SecretKey,
                    ApiKey = result.ApiKey,
                    UserId = result.UserId,
                    ExchangeId = result.ExchangeId
                };

                return userKey;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public async Task<BalanceInfo> CreateUserKey(CreateUserKeyInput input)
        {
            try
            {
                
                string avialableBalance = string.Empty;
                BalanceInfo info = new BalanceInfo();
                if (IsAlreadyExists(input.ApiKey, input.SecretKey, input.UserId, input.ExchangeId))
                {
                    info.Status = "ApiKey and secretKey already exists.";
                    info.AvailableBalance = "0.00";
                    return info;
                }
                if (await ValidateFromExchangeUserKeys(input.ApiKey, input.SecretKey))
                {
                    var userKey = ObjectMapper.Map<UserKey>(input);
                    var result =await _userKeyRepository.InsertAsync(userKey);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    CoreUserManager coreUserManager = new CoreUserManager();
                    var loginUser = _userManager.Users.Where(x => x.Id == result.CreatorUserId).FirstOrDefault();
                    var TotalDays = coreUserManager.GetBotLimitFromMLM(loginUser.UserName);

                    Products.Dto.UserProductSearch userProductSearch = new Products.Dto.UserProductSearch() { UserId = Convert.ToInt32(result.CreatorUserId) };
                    var payments = _userProductsPaymentRecordsDomainService.GetUserProductsPaymentsByUserID(userProductSearch);
                    if (payments != null && payments.Items != null && payments.Items.ToList().Count > 0)
                    {
                        foreach (var item in payments.Items)
                        {
                            if (item.Type == "confirmed")
                            {
                                Products.Dto.ProductSearch productSearch = new Products.Dto.ProductSearch() { Id = item.ProductId };
                                var BotDays = _productAppService.GetProduct(productSearch).Result.Duration;
                                TotalDays = TotalDays + BotDays;
                            }
                        }
                    }
                    if (TotalDays > 0)
                    {
                        loginUser.ExpiryDate = DateTime.UtcNow.AddDays(TotalDays);
                        await _userManager.UpdateAsync(loginUser);
                    }

                    info.AvailableBalance = await _exchangeInfo.GetBTCBlance("Binance", input.ApiKey, input.SecretKey);
                    info.Status = "Success";
                  
                }               
                else
                {
                    info.AvailableBalance = "0.00";
                    info.Status = "Invalid API-Key";

                }
                return info;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<int> GetTotalDaysByUserId(UserKeySearchByUserIdInput input)
        {
            try
            {
                var TotalDays = 0;

                var userKey = _userKeyRepository
             .GetAll()
             .Where(

                 r => r.UserId == input.UserId
             )
             .ToList();

                if (userKey == null || userKey.Count == 0 )
                {
                    CoreUserManager coreUserManager = new CoreUserManager();
                    var loginUser = _userManager.Users.Where(x => x.Id == input.UserId).FirstOrDefault();
                     TotalDays = coreUserManager.GetBotLimitFromMLM(loginUser.UserName);

                    Products.Dto.UserProductSearch userProductSearch = new Products.Dto.UserProductSearch() { UserId = Convert.ToInt32(input.UserId) };
                    var payments = _userProductsPaymentRecordsDomainService.GetUserProductsPaymentsByUserID(userProductSearch);
                    if (payments != null && payments.Items != null && payments.Items.ToList().Count > 0)
                    {
                        foreach (var item in payments.Items)
                        {
                            if (item.Type == "confirmed")
                            {
                                Products.Dto.ProductSearch productSearch = new Products.Dto.ProductSearch() { Id = item.ProductId };
                                var BotDays = _productAppService.GetProduct(productSearch).Result.Duration;
                                TotalDays = TotalDays + BotDays;
                            }
                        }
                    }
                }              
                return TotalDays;

            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        public async Task<BalanceInfo> EditUserKey(EditUserKeyInput input)
        {
            try
            {
                BalanceInfo info = new BalanceInfo();
                if (IsAlreadyExists(input.ApiKey, input.SecretKey, input.UserId, input.ExchangeId))
                {
                    info.Status = "ApiKey and secretKey already exists.";
                    info.AvailableBalance = "0.00";
                    return info;
                }
                

                if (await ValidateFromExchangeUserKeys(input.ApiKey, input.SecretKey))
                {

                    var userKey = await _userKeyRepository.GetAsync(input.Id);
                    userKey.SecretKey = input.SecretKey;
                    userKey.ApiKey = input.ApiKey;
                    userKey.UserId = input.UserId;
                    userKey.ExchangeId = input.ExchangeId;
                    string avialableBalance = string.Empty;

                    info.AvailableBalance = await _exchangeInfo.GetBTCBlance("Binance", input.ApiKey, input.SecretKey);
                    info.Status = "Success";
                    await _userKeyRepository.UpdateAsync(userKey);
                }
                else
                {
                    info.AvailableBalance = "0.00";
                    info.Status = "Invalid API-Key";

                }
                
                return info;


            }
            catch (Exception e)
            {
                await Task.FromResult(0);
                return null;
            }

        }
       

        public async Task<List<Binance.Net.Objects.BinanceBalance>> GetUserBalanceDetail(UserKeySearchByUserIdInput input)
        {
            try
            {
                List<Binance.Net.Objects.BinanceBalance> balanceResponse = new List<Binance.Net.Objects.BinanceBalance>();
                var userKey = _userKeyRepository
                    .GetAll()
                    .Where(

                        r => r.UserId == input.UserId
                    )
                    .FirstOrDefault();
                if (userKey != null)
                {
                    //var client = new BinanceClient(new ClientConfiguration()
                    //{
                    //    ApiKey = userKey.ApiKey,
                    //    SecretKey = userKey.SecretKey,
                    //});

                    Binance.Net.BinanceClient lObj_BinanceClient;
                    lObj_BinanceClient = new Binance.Net.BinanceClient();
                    lObj_BinanceClient.SetApiCredentials(userKey.ApiKey, userKey.SecretKey);

                    balanceResponse = await _exchangeInfo.GetDetailBalance("Binance", userKey.ApiKey, userKey.SecretKey);
                    if (balanceResponse != null && balanceResponse.Count == 1 && balanceResponse.Where(m => m.Free == -1 && m.Asset.Trim().ToLower().StartsWith("error")).Any())
                    {
                        Logger.Info("balance Response  of UserId='"+input.UserId+"'   Apikey='"+ userKey.ApiKey + "'  SecretKey ='" + userKey.SecretKey + "'   Error= " + balanceResponse.First().Asset);
                    }
                }
                return balanceResponse;
            }
            catch (Exception)
            {
                return null;
            }

        }
        
        public async Task<BalanceInfo> GetUserBalance(UserKeySearchByUserIdInput input)
        {
            try
            {
                BalanceInfo info = new BalanceInfo();

                var userKey = _userKeyRepository
                    .GetAll()
                    .Where(

                        r => r.UserId == input.UserId
                    )
                    .FirstOrDefault();
                if (userKey != null)
                {
                    var client = new BinanceClient(new ClientConfiguration()
                    {
                        ApiKey = userKey.ApiKey,
                        SecretKey = userKey.SecretKey,
                    });

                    //var MarketsResult = _marketRepository.GetAll().Where(x => x.Name.Contains("/BTC")).Select(x=>x.Name).ToArray();
                    //info.AvailableBalance = await _exchangeInfo.GetBTCBlance("Binance", userKey.ApiKey, userKey.SecretKey, MarketsResult);

                    info.AvailableBalance = await _exchangeInfo.GetBTCBlance("Binance", userKey.ApiKey, userKey.SecretKey);
                    info.Status = "Success";
                }
                return info;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<Binance.Net.Objects.BinanceTradeRuleResult> GetExchangeTradingRules(TradingParameterInput input)
        {
            try
            {
                Binance.Net.Objects.BinanceTradeRuleResult Result = new Binance.Net.Objects.BinanceTradeRuleResult();

                //Result = await _exchangeInfo.GetExTradingRules(input.symbol, input.Quantity, input.Price);

                //var rulesCheck = await Binance.Net.BinanceClient.CheckTradeRules(symbol, quantity, price, type).ConfigureAwait(false);

                Binance.Net.BinanceClient binanceClient = new Binance.Net.BinanceClient();

                Result = await binanceClient.CheckTradeRules(input.Pair, input.quantity, input.price, Binance.Net.Objects.OrderType.Market).ConfigureAwait(false);

                if (Result != null)
                {
                    return Result;
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
       
        public async Task<string> GetTickerValue(TradingParameterInput input)
        {
            try
            {

               var userKey = _userKeyRepository
              .GetAll()
              .Where(
                  r => r.UserId == input.UserId
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

                var tickerBtc = await lObj_BinanceClient.GetPriceAsync(input.Pair);
                return Convert.ToString(tickerBtc.Data.Price);

                //SymbolPriceChangeTickerResponse tickerSymbolResult = new SymbolPriceChangeTickerResponse();
                //tickerSymbolResult = await client.GetDailyTicker(input.Pair);
                //return Convert.ToString(tickerSymbolResult.AskPrice);
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public async Task<List<Binance.Net.Objects.BinanceBalance>> GetUserBalanceDetailDynamically(UserKeySearchByUserIdInput input)
        {
            try
            {
                List<Binance.Net.Objects.BinanceBalance> balanceResponse = new List<Binance.Net.Objects.BinanceBalance>();
                var userKey = _userKeyRepository
                    .GetAll()
                    .Where(

                        r => r.UserId == input.UserId
                    )
                    .FirstOrDefault();
                if (userKey != null)
                {
                    Binance.Net.BinanceClient lObj_BinanceClient;
                    lObj_BinanceClient = new Binance.Net.BinanceClient();
                    lObj_BinanceClient.SetApiCredentials(userKey.ApiKey, userKey.SecretKey);
                    var currencyNames = _tradingCurrenciesRepository.GetAllList(x=>x.IsActive!=false).Select(c=>c.CurrencyName).ToArray();
                    balanceResponse = await _exchangeInfo.GetDetailBalance(Common.ExchangeName.Binance.ToString(), userKey.ApiKey, userKey.SecretKey, currencyNames);

                }
                return balanceResponse;
            }
            catch (Exception)
            {
                return null;
            }

        }

        #endregion

        #region Hepler

        private async Task<bool> ValidateFromExchangeUserKeys(string apiKey, string userKey)
        {
            try
            {
                bool isValid = false;
                var client = new BinanceClient(new ClientConfiguration()
                {
                    ApiKey = apiKey,
                    SecretKey = userKey,
                });
                // Start User Data Stream, ping and close
                var userData = await client.StartUserDataStream();
                await client.KeepAliveUserDataStream(userData.ListenKey);
                await client.CloseUserDataStream(userData.ListenKey);
                if (userData.ListenKey != null && userData.ListenKey != string.Empty)
                {
                    isValid = true;
                }

                return isValid;
            }
            catch (Exception e)
            {
                if (e.Message == "")
                {
                    return false;
                }
                throw;
            }

            
        }
        private bool IsAlreadyExists(string apiKey, string secretKey, long userId, int exchangeId)
        {
            bool isExists = true;
            var isKeyExists = _userKeyRepository
                .GetAll()
                .Where(

                    r => r.UserId == userId && r.ApiKey == apiKey && r.SecretKey == secretKey && r.ExchangeId == exchangeId
                )
                .FirstOrDefault();
            if (isKeyExists != null)
            {
                isExists = true;
            }
            else
            {
                isExists = false;
            }
            return isExists;
        }
        #endregion
    }
}
