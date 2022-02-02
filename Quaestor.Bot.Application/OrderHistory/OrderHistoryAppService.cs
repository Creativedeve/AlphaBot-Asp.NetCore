using Abp.Authorization;
using Abp.Domain.Repositories;
using Quaestor.Bot.Markets;
using Quaestor.Bot.OrderHistory.Dto;
using Quaestor.Bot.PurchaseOrderRecords;
using Quaestor.Bot.TradeProfitRates;
using Quaestor.Bot.UserKeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quaestor.Bot.OrderHistory
{
    [AbpAuthorize]
    public class OrderHistoryAppService : BotAppServiceBase, IOrderHistoryAppService
    {
        #region Properties
        private readonly IRepository<PurchaseOrderRecord, int> _orderHistoryRecordRepository;
        private readonly IRepository<UserKey, int> _userKeyRepository;
        private readonly IRepository<Market, int> _marketRepository;
        private readonly IRepository<TradeProfitSellInfo, int> _orderHistorySaleRecordRepository;
        #endregion


        #region Constructor
        public OrderHistoryAppService(IRepository<PurchaseOrderRecord, int> repository, IRepository<UserKey, int> userKeyRepository, IRepository<Market, int> marketRepository,IRepository<TradeProfitSellInfo, int> orderHistorySaleRecordRepository)
        {
            _orderHistoryRecordRepository = repository;
            _userKeyRepository = userKeyRepository;
            _marketRepository = marketRepository;
            _orderHistorySaleRecordRepository = orderHistorySaleRecordRepository;
        }
        #endregion


        #region Methods

        public async Task<List<Binance.Net.Objects.BinanceOrder>> GetUserOrderHistory(UserOrderHistoryInput input)
        {
            try
            {
                var userKey = _userKeyRepository.GetAll().Where(u => u.UserId == input.UserId).FirstOrDefault();

                Binance.Net.BinanceClient binanceClient;

                binanceClient = new Binance.Net.BinanceClient();
                binanceClient.SetApiCredentials(userKey.ApiKey, userKey.SecretKey);

                List<Binance.Net.Objects.BinanceOrder> binanceOrdersDetail = null;
                List<PurchaseOrderRecord> lstPurchaseOrderRecordDto = GetUserOrderHistoryInnerMethod(input);
                if (lstPurchaseOrderRecordDto != null && lstPurchaseOrderRecordDto.Count > 0)
                {
                    var uniqueMarketsName = lstPurchaseOrderRecordDto.Select(x => x.MarketName).Distinct();
                    binanceOrdersDetail = new List<Binance.Net.Objects.BinanceOrder>();
                    foreach (string marketName in uniqueMarketsName)
                    {
                        var allMarketOrderResult = await binanceClient.GetAllOrdersAsync(marketName);
                        if (allMarketOrderResult != null && allMarketOrderResult.Data.ToList().Count > 0)
                        {                            
                            foreach (PurchaseOrderRecord item in lstPurchaseOrderRecordDto.Where(m => m.MarketName == marketName))
                            {
                                if (item.OrderId > 0)
                                {
                                    var OrderResult = allMarketOrderResult.Data.ToList().Where(m => m.OrderId == item.OrderId).ToList();

                                    if (OrderResult != null)
                                    {
                                        for (int i = 0; i < OrderResult.Count; i++)
                                        {
                                            if (OrderResult[i].Price <= 0)
                                            {
                                                OrderResult[i].Price = Convert.ToDecimal(item.FirstBuyRate);
                                            }
                                            binanceOrdersDetail.Add(OrderResult[i]);
                                        }
                                    }
                                }

                                foreach (PurchaseOrderRecordDetail itemDetail in item.PurchaseOrderRecordDetails)
                                {

                                    if (itemDetail.OrderId > 0)
                                    {
                                        var DetailOrderResult = allMarketOrderResult.Data.ToList().Where(m => m.OrderId == itemDetail.OrderId).ToList();

                                        if (DetailOrderResult != null)
                                        {
                                            for (int i = 0; i < DetailOrderResult.Count; i++)
                                            {
                                                if (DetailOrderResult[i].Price <= 0)
                                                {
                                                    DetailOrderResult[i].Price = Convert.ToDecimal(itemDetail.ReBuyRate);
                                                }
                                                binanceOrdersDetail.Add(DetailOrderResult[i]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return binanceOrdersDetail;
            }
            catch (Exception)
            {
                return null;
            }

        }

        private List<PurchaseOrderRecord> GetUserOrderHistoryInnerMethod(UserOrderHistoryInput input)
        {
            try
            {
                var purchaseOrder = _orderHistoryRecordRepository.GetAllIncluding(p => p.PurchaseOrderRecordDetails).ToList().Where(p => p.CreatorUserId == input.UserId).ToList();
                return purchaseOrder;
            }
            catch (Exception)
            {
                return null;
            }

        }


        public async Task<List<Binance.Net.Objects.BinanceOrder>> GetUserSaleOrderHistory(UserOrderHistoryInput input)
        {
            try
            {
                var userKey = _userKeyRepository.GetAll().Where(u => u.UserId == input.UserId).FirstOrDefault();

                Binance.Net.BinanceClient binanceClient;

                binanceClient = new Binance.Net.BinanceClient();
                binanceClient.SetApiCredentials(userKey.ApiKey, userKey.SecretKey);

                List<Binance.Net.Objects.BinanceOrder> lstBinanceOrders = null;
                List<TradeProfitSellInfo> lstTradeProfitSellInfo = GetUserSaleOrderHistoryInnerMethod(input);           
                if (lstTradeProfitSellInfo != null && lstTradeProfitSellInfo.Count > 0)
                {
                    var uniqueMarketsIds = lstTradeProfitSellInfo.Select(x => x.MarketId).Distinct();
                    lstBinanceOrders = new List<Binance.Net.Objects.BinanceOrder>();
                    foreach (int marketId in uniqueMarketsIds)
                    {
                        var markerName = _marketRepository.Get(marketId).Name.Replace("/", "");
                        var allMarketSaleResult = await binanceClient.GetAllOrdersAsync(markerName);
                        if (allMarketSaleResult != null && allMarketSaleResult.Data.ToList().Count > 0)
                        {                        
                            foreach (TradeProfitSellInfo item in lstTradeProfitSellInfo.Where(m => m.MarketId == marketId))
                            {
                                if (item.OrderId > 0)
                                {
                                    var OrderResult = allMarketSaleResult.Data.ToList().Where(m => m.OrderId == item.OrderId).ToList();

                                    if (OrderResult != null)
                                    {
                                        for (int i = 0; i < OrderResult.Count; i++)
                                        {
                                            if (OrderResult[i].Price <= 0)
                                            {
                                                OrderResult[i].Price = Convert.ToDecimal(item.SellRate);
                                            }
                                            lstBinanceOrders.Add(OrderResult[i]);
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                return lstBinanceOrders;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private List<TradeProfitSellInfo> GetUserSaleOrderHistoryInnerMethod(UserOrderHistoryInput input)
        {
            try
            {                
                var tradeProfitSellInfo = _orderHistorySaleRecordRepository.GetAll().Where(p => p.CreatorUserId == input.UserId).ToList();
                return tradeProfitSellInfo;
            }
            catch (Exception ex)
            {
                return null;
            }

        }



        #endregion

    }
}
