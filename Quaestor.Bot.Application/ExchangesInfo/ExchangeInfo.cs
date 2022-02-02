using BinanceExchange.API.Client;
using BinanceExchange.API.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quaestor.Bot.ExchangesInfo
{
    public class ExchangeInfo
    {
        //public async Task<string> GetBTCBlance(string exchangeName, string apiKey, string apiSecret)
        //{
        //    string avialableBalance = string.Empty;
        //    decimal balance = 0;

        //    string Res = "Failed";
        //    try
        //    {
        //        switch (exchangeName)
        //        {

        //            case "Binance":
        //                var client = new BinanceClient(new ClientConfiguration()
        //                {
        //                    ApiKey = apiKey,
        //                    SecretKey = apiSecret

        //                });

        //                var accountInformation = await client.GetAccountInformation();

        //                if (accountInformation != null)
        //                {
        //                    if (accountInformation.Balances.Count <= 0)
        //                    {
        //                        avialableBalance = "0.00";
        //                    }
        //                    else
        //                    {
        //                        var result = accountInformation.Balances.Where(c => c.Free > 0).ToList();

        //                        foreach (var item in result)
        //                        {
        //                            if (balance == 0)
        //                            {
        //                                if (item.Asset == "BTC")
        //                                {
        //                                    balance = item.Free;
        //                                }
        //                                else
        //                                {
        //                                    var price = await GetBalanceInBTC(item.Asset + "BTC", client);
        //                                    balance = Convert.ToDecimal(price) * item.Free;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (item.Asset == "BTC")
        //                                {
        //                                    balance += Convert.ToDecimal(item.Free);
        //                                }
        //                                else
        //                                {
        //                                    var price = await GetBalanceInBTC(item.Asset + "BTC", client);
        //                                    balance += Convert.ToDecimal(price) * item.Free;
        //                                }
        //                            }
        //                        }
        //                        avialableBalance = Convert.ToString(balance);
        //                    }

        //                }
        //                else
        //                    avialableBalance = Res;
        //                break;

        //        }
        //        return avialableBalance;
        //    }
        //    catch (Exception)
        //    {
        //        return avialableBalance = Res;
        //    }
        //}


        public async Task<string> GetBTCBlance(string exchangeName, string apiKey, string apiSecret)
        {
            string avialableBalance = string.Empty;
            decimal balance = 0;

            string Res = "Failed";
            try
            {
                switch (exchangeName)
                {

                    case "Binance":                       
                        Binance.Net.BinanceClient lObj_BinanceClient;
                        lObj_BinanceClient = new Binance.Net.BinanceClient();
                        lObj_BinanceClient.SetApiCredentials(apiKey, apiSecret);
                        decimal btcTotal = 0;
                        var tickers = lObj_BinanceClient.GetAllPrices();
                        var accountInformation = await lObj_BinanceClient.GetAccountInfoAsync();
                        var assetDetails = lObj_BinanceClient.GetAssetDetails();
                        if (accountInformation != null)
                        {
                           
                            foreach (var item in accountInformation.Data.Balances)
                            {
                                decimal btcValue = 0;
                                if (item.Free > 0)
                                {
                                    try
                                    {
                                        //if (item.Asset == "BTC") btcValue = item.Total;
                                        //else if (item.Asset == "USDT") btcValue = item.Total / lObj_BinanceClient.GetPrice("BTCUSDT").Data.Price;
                                        //else btcValue = item.Total * lObj_BinanceClient.GetPrice(item.Asset + "BTC").Data.Price;
                                        //btcTotal += btcValue;
                                        if (!assetDetails.Data.Where(a => a.Key == item.Asset).First().Value.DepositStatus)
                                        {
                                            continue;
                                        }
                                        if (item.Asset == "BTC") btcValue = item.Total;
                                        else if (item.Asset == "USDT") btcValue = item.Total / tickers.Data.ToList().Where(x => x.Symbol == "BTCUSDT").FirstOrDefault().Price;
                                        else btcValue = item.Total * tickers.Data.ToList().Where(x => x.Symbol == item.Asset + "BTC").FirstOrDefault().Price;
                                        btcTotal += btcValue;
                                    }
                                    catch (Exception)
                                    {
                                    }
                                  
                                }
                            }
                            avialableBalance = btcTotal.ToString();
                        }
                        else
                            avialableBalance = Res;
                        break;

                }
                return avialableBalance;
            }
            catch (Exception e)
            {
                return avialableBalance = Res;
            }
        }
        public async Task<string> GetBTCBlance(string exchangeName, string apiKey, string apiSecret,string[] SupportedMarkets)
        {
            string avialableBalance = string.Empty;
            decimal balance = 0;
            //List<string> ActualSupportedMarkets = new List<string>();
            //foreach (var item in SupportedMarkets)
            //{
            //    ActualSupportedMarkets.Add(item.Split("/BTC")[0]);
            //}

            string Res = "Failed";
            try
            {
                switch (exchangeName)
                {

                    case "Binance":
                        //var client = new BinanceClient(new ClientConfiguration()
                        //{
                        //    ApiKey = apiKey,
                        //    SecretKey = apiSecret

                        //});

                        Binance.Net.BinanceClient lObj_BinanceClient;
                        lObj_BinanceClient = new Binance.Net.BinanceClient();
                        lObj_BinanceClient.SetApiCredentials(apiKey, apiSecret);

                        var accountInformation = await lObj_BinanceClient.GetAccountInfoAsync();
                        if (accountInformation != null)
                        {
                            if (accountInformation.Data.Balances.Count <= 0)
                            {
                                avialableBalance = "0.00";
                            }
                            else
                            {
                                var result = accountInformation.Data.Balances.Where(c => c.Free > 0).ToList();

                                foreach (var item in result)
                                {
                                    //if (!ActualSupportedMarkets.Contains(item.Asset))
                                    //{
                                    //    continue;
                                    //}
                                   
                                    if (balance == 0)
                                    {
                                        if (item.Asset == "BTC")
                                        {
                                            balance = item.Free;
                                        }
                                        else
                                        {
                                            try
                                            {
                                                var tickerBtc = await lObj_BinanceClient.GetPriceAsync(item.Asset + "BTC");
                                                //var price = await GetBalanceInBTC(item.Asset + "BTC", lObj_BinanceClient);
                                                balance += Convert.ToDecimal(tickerBtc.Data.Price) * item.Free;
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item.Asset == "BTC")
                                        {
                                            balance += Convert.ToDecimal(item.Free);
                                        }
                                        else
                                        {
                                            //var price = await GetBalanceInBTC(item.Asset + "BTC", client);
                                            //balance += Convert.ToDecimal(price) * item.Free;
                                            try
                                            {
                                                var tickerBtc = await lObj_BinanceClient.GetPriceAsync(item.Asset + "BTC");
                                                //var price = await GetBalanceInBTC(item.Asset + "BTC", lObj_BinanceClient);
                                                balance += Convert.ToDecimal(tickerBtc.Data.Price) * item.Free;
                                            }
                                            catch (Exception)
                                            {                                               
                                            }
                                          
                                        }
                                    }
                                }
                                avialableBalance = Convert.ToString(balance);
                            }
                        }
                        else
                            avialableBalance = Res;
                        break;

                }
                return avialableBalance;
            }
            catch (Exception ex)
            {
                return avialableBalance = Res;
            }
        }

        //public async Task<List<BalanceResponse>> GetDetailBalance(string exchangeName, string apiKey, string apiSecret)
        //{
        //    List<BalanceResponse> balanceResponse = null;

        //    try
        //    {
        //        balanceResponse = new List<BalanceResponse>();


        //        switch (exchangeName)
        //        {

        //            case "Binance":
        //                var client = new BinanceClient(new ClientConfiguration()
        //                {
        //                    ApiKey = apiKey,
        //                    SecretKey = apiSecret

        //                });

        //                var accountInformation = await client.GetAccountInformation();
        //                balanceResponse = accountInformation.Balances.Where(c => c.Free > 0).ToList();

        //                break;
        //        }
        //        return balanceResponse;
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }

        //}

        public async Task<List<Binance.Net.Objects.BinanceBalance>> GetDetailBalance(string exchangeName, string apiKey, string apiSecret)
        {
            List<Binance.Net.Objects.BinanceBalance> balanceResponse = null;

            try
            {
            
                balanceResponse = new List<Binance.Net.Objects.BinanceBalance>();

                switch (exchangeName)
                {

                    case "Binance":
                      
                        Binance.Net.BinanceClient lObj_BinanceClient;
                        lObj_BinanceClient = new Binance.Net.BinanceClient();
                        lObj_BinanceClient.SetApiCredentials(apiKey, apiSecret);

                      
                        var accountInformation = await lObj_BinanceClient.GetAccountInfoAsync();
                        balanceResponse = accountInformation.Data.Balances.Where(c => c.Free > 0).ToList();

                        break;
                }
                return balanceResponse;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<List<Binance.Net.Objects.BinanceBalance>> GetDetailBalance(string exchangeName, string apiKey, string apiSecret,string[] tradingCurrencies)
        {
            List<Binance.Net.Objects.BinanceBalance> balanceResponse = null;

            try
            {

                balanceResponse = new List<Binance.Net.Objects.BinanceBalance>();

                switch (exchangeName)
                {

                    case "Binance":

                        Binance.Net.BinanceClient lObj_BinanceClient;
                        lObj_BinanceClient = new Binance.Net.BinanceClient();
                        lObj_BinanceClient.SetApiCredentials(apiKey, apiSecret);


                        var accountInformation = await lObj_BinanceClient.GetAccountInfoAsync();
                        if (accountInformation != null && accountInformation.Data!=null && accountInformation.Data.Balances !=null  && accountInformation.Data.Balances.Count>0)
                            balanceResponse = accountInformation.Data.Balances.Where(c => tradingCurrencies.Contains(c.Asset)).ToList();
                        else
                        {
                            Binance.Net.Objects.BinanceBalance binanceBalance = new Binance.Net.Objects.BinanceBalance
                            {
                                Asset = "Error Unable to get user balance info from binanace",
                                Free = -1,
                                Locked=-1// it's indicating some error occured while getting balance
                            };
                            balanceResponse.Add(binanceBalance);
                        }

                        break;
                }
                return balanceResponse;
            }
            catch (Exception e)
            {
                Binance.Net.Objects.BinanceBalance binanceBalance = new Binance.Net.Objects.BinanceBalance
                {
                    Asset = "Error "+e.Message,
                    Free = -1,
                    Locked = -1// it's indicating some error occured while getting balance
                };
                balanceResponse.Add(binanceBalance);
                return balanceResponse;
            }

        }

        private async Task<string> GetBalanceInBTC(string asset, BinanceClient binanceClient)
        {
            try
            {
                SymbolPriceChangeTickerResponse tickerSymbolResult = new SymbolPriceChangeTickerResponse();
                tickerSymbolResult = await binanceClient.GetDailyTicker(asset);

                return Convert.ToString(tickerSymbolResult.AskPrice);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<string> GetTickerValue(string asset, BinanceClient binanceClient)
        {
            try
            {
                SymbolPriceChangeTickerResponse tickerSymbolResult = new SymbolPriceChangeTickerResponse();
                tickerSymbolResult = await binanceClient.GetDailyTicker(asset);
                return Convert.ToString(tickerSymbolResult.AskPrice);
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}

