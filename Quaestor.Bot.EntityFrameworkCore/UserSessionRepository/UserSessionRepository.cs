using Abp.Data;
using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quaestor.Bot.DashBoard;
using Quaestor.Bot.EntityFrameworkCore;
using Quaestor.Bot.EntityFrameworkCore.Repositories;
using Quaestor.Bot.PackagesDetail;
using Quaestor.Bot.UserSessions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
//using Quaestor.Bot.PurchaseOrderRecords;


namespace Quaestor.Bot.UserSessionRepository
{
    public class UserSessionRepository : BotRepositoryBase<UserSession>, IUserSessionRepository
    {
        private readonly IActiveTransactionProvider _transactionProvider;

        public UserSessionRepository(IDbContextProvider<BotDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider)
        {
            _transactionProvider = transactionProvider;
        }
        public List<RebuyDetail> GetAllRebuy()
        {
            EnsureConnectionOpen();
            var result = new List<RebuyDetail>();
            using (var command = CreateCommand("usp_s_AllRebuysRecords", CommandType.StoredProcedure))
            {
                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        RebuyDetail rebuyDetail = new RebuyDetail();
                        rebuyDetail.PurchaseOrderRecordId = Convert.ToInt32(dataReader["PurchaseOrderRecordId"]);
                        rebuyDetail.MarketName = Convert.ToString(dataReader["MarketName"]);
                        rebuyDetail.UserId = Convert.ToInt32(dataReader["UserId"]);
                        rebuyDetail.UserSessionDetailId = Convert.ToInt32(dataReader["UserSessionDetailId"]);
                        rebuyDetail.ReBuyWith = Convert.ToDecimal(dataReader["RebuyValue"]);
                        rebuyDetail.RebuyRate = Convert.ToDecimal(dataReader["RebuyRate"]);
                        rebuyDetail.ReBuySequence = Convert.ToInt32(dataReader["ReBuySequence"]);
                        rebuyDetail.MarketId = Convert.ToInt32(dataReader["MarketId"]);
                        result.Add(rebuyDetail);
                    }                   
                }
            }
            return result;
        }
        public List<FirstBuyDetail> GetAllFirstBuy(int? userId)
        {
            EnsureConnectionOpen();
            var result = new List<FirstBuyDetail>();
            using (var command = CreateCommand("usp_s_FirstBuyRecords", CommandType.StoredProcedure))
            {
                command.Parameters.Add(new SqlParameter("@UserId", userId));

                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        FirstBuyDetail firstbuyDetail = new FirstBuyDetail();
                        firstbuyDetail.UserId = Convert.ToInt32(dataReader["UserId"]);
                        firstbuyDetail.MarketName = Convert.ToString(dataReader["MarketName"]);
                        firstbuyDetail.MarketId = Convert.ToInt32(dataReader["MarketId"]);
                        firstbuyDetail.FirstBuyWith = Convert.ToDecimal(dataReader["FirstBuyWith"]);
                        firstbuyDetail.UserSessionDetailId = Convert.ToInt32(dataReader["UserSessionDetailId"]);
                        result.Add(firstbuyDetail);
                    }                 
                }
            }
            return result;
        }
        private DbCommand CreateCommand(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            var command = Context.Database.GetDbConnection().CreateCommand();

            command.CommandText = commandText;
            command.CommandType = commandType;
            command.Transaction = GetActiveTransaction();

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return command;
        }

        private void EnsureConnectionOpen()
        {
            var connection = Context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        private DbTransaction GetActiveTransaction()
        {
            return (DbTransaction)_transactionProvider.GetActiveTransaction(new ActiveTransactionProviderArgs
            {
                {"ContextType", typeof(BotDbContext) },
                {"MultiTenancySide", MultiTenancySide }
            });
        }

        public bool IsAlreadyBought(string BuyType, int MarketId, int? UserSessionDetailId = null, int? ReBuySequence = null, int? PurchaseOrderRecordId = null)
        {
            bool IsAlreadyBought = false;
            EnsureConnectionOpen();
            using (var command = CreateCommand("usp_s_IsAlreadyBought", CommandType.StoredProcedure))
            {
                command.Parameters.Add(new SqlParameter("@BuyType", BuyType));
                command.Parameters.Add(new SqlParameter("@MarketId", MarketId));
                command.Parameters.Add(new SqlParameter("@UserSessionDetailId", UserSessionDetailId));
                command.Parameters.Add(new SqlParameter("@ReBuySequence", ReBuySequence));
                command.Parameters.Add(new SqlParameter("@PurchaseOrderRecordId", PurchaseOrderRecordId));
                command.Parameters.Add(new SqlParameter("@IsAlreadyBought", SqlDbType.Bit) { Direction = ParameterDirection.Output, Value = false });
                command.ExecuteNonQuery();
                IsAlreadyBought = (bool)command.Parameters["@IsAlreadyBought"].Value;
            }
            return IsAlreadyBought;
        }

        public async Task<List<DashboardResult>> GetUserDashboardDetail(int? UserId)
        {

            try
            {
                EnsureConnectionOpen();

                SqlParameter[] parameters =
                          {
                                     new SqlParameter("@UserId", SqlDbType.BigInt)
                          };
                parameters[0].Value = UserId;
                List<DashboardResult> result = new List<DashboardResult>();
                using (var command = CreateCommand("usp_s_UserDashBoard", CommandType.StoredProcedure, parameters))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            DashboardResult dashboardResult = new DashboardResult();
                            dashboardResult.MarketName = Convert.ToString(dataReader["MarketName"]);
                            dashboardResult.MarketId = Convert.ToInt32(dataReader["MarketId"]);
                            dashboardResult.Quantity = Convert.ToDecimal(dataReader["Quantity"]);
                            dashboardResult.AverageValue = Convert.ToDecimal(dataReader["AverageValue"]);
                            dashboardResult.TotalBuy = Convert.ToInt32(dataReader["TotalBuy"]);
                            dashboardResult.NextBuyPoint = Convert.ToDecimal(dataReader["NextBuyPoint"]);
                            dashboardResult.TradeProfitSaleRate = Convert.ToDecimal(dataReader["TradeProfitSaleRate"]);
                            dashboardResult.UserSessionId = Convert.ToInt32(dataReader["UserSessionId"]);
                            dashboardResult.IsSessionClosed = Convert.ToBoolean(dataReader["IsSessionClosed"]);
                            result.Add(dashboardResult);
                        }
                       
                    }
                }
                return result;
            }
            catch (Exception)
            {

                return new List<DashboardResult>();
            }
        }

        public List<TradeProfitDetailResult> GetAllTradeProfitRecords()
        {
            EnsureConnectionOpen();
            var result = new List<TradeProfitDetailResult>();
            using (var command = CreateCommand("usp_s_AllTradeProfitRecords", CommandType.StoredProcedure))
            {
                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        TradeProfitDetailResult rebuyDetail = new TradeProfitDetailResult();
                        rebuyDetail.PurchaseOrderRecordId = Convert.ToInt32(dataReader["PurchaseOrderRecordId"]);
                        rebuyDetail.MarketName = Convert.ToString(dataReader["MarketName"]);
                        rebuyDetail.UserId = Convert.ToInt32(dataReader["UserId"]);
                        rebuyDetail.TradeProfitRateId = Convert.ToInt32(dataReader["TradeProfitRateId"]);
                        rebuyDetail.SaleRate = Convert.ToDecimal(dataReader["SaleRate"]);
                        rebuyDetail.CurrencyPurchased = Convert.ToDecimal(dataReader["CurrencyPurchased"]);
                        rebuyDetail.MarketId = Convert.ToInt32(dataReader["MarketId"]);
                        result.Add(rebuyDetail);
                    }

                   
                }
            }
            return result;
        }


        public List<BuyStopUpDownResult> GetAllBuyStopUpAndDown()
        {
            EnsureConnectionOpen();
            var result = new List<BuyStopUpDownResult>();
            using (var command = CreateCommand("usp_s_BuyUpBuyDownRecords", CommandType.StoredProcedure))
            {
                using (var dataReader = command.ExecuteReader())
                {
                    
                    while (dataReader.Read())
                    {
                        BuyStopUpDownResult buyStopUpDownDetail = new BuyStopUpDownResult()
                        {
                            BuyStopUpPercentage = Convert.ToDecimal(dataReader["BuyStopUpPercentage"]),
                            BuyStopUpValue = Convert.ToDecimal(dataReader["BuyStopUpValue"]),
                            BuyStopUpRate = Convert.ToDecimal(dataReader["BuyStopUpRate"]),
                            SellRate = Convert.ToDecimal(dataReader["SellRate"]),
                            BuyStopDownPercentage = Convert.ToDecimal(dataReader["BuyStopDownPercentage"]),
                            BuyStopDownValue = Convert.ToDecimal(dataReader["BuyStopDownValue"]),
                            BuyStopDownRate = Convert.ToDecimal(dataReader["BuyStopDownRate"]),
                            MarketId = Convert.ToInt32(dataReader["MarketId"]),
                            CreatorUserId = Convert.ToInt32(dataReader["CreatorUserId"]),
                            UserSessionDetailId = Convert.ToInt32(dataReader["UserSessionDetailId"]),
                            PurchaseOrderRecordId = Convert.ToInt32(dataReader["PurchaseOrderRecordId"]),
                            MarketName = Convert.ToString(dataReader["MarketName"]),
                            FirstBuyWith = Convert.ToDecimal(dataReader["FirstBuyWith"])
                        };
                        result.Add(buyStopUpDownDetail);
                    }                  
                }
            }
            return result;
        }

        public TradeProfitDetailResult SellAllBoughtRecordsAgainstUser(Int64 UserId, int PurchaseOrderRecordId, int MarketId)
        {
            EnsureConnectionOpen();
            using (var command = CreateCommand("usp_s_SellOnSessionClosed", CommandType.StoredProcedure))
            {
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                command.Parameters.Add(new SqlParameter("@PurchaseOrderRecordId", PurchaseOrderRecordId));
                command.Parameters.Add(new SqlParameter("@MarketId", MarketId));
                TradeProfitDetailResult rebuyDetail = new TradeProfitDetailResult();
                using (var dataReader = command.ExecuteReader())
                {
                   
                    if (dataReader.Read())
                    {

                        rebuyDetail.PurchaseOrderRecordId = Convert.ToInt32(dataReader["PurchaseOrderRecordId"]);
                        rebuyDetail.MarketName = Convert.ToString(dataReader["MarketName"]);
                        rebuyDetail.UserId = Convert.ToInt32(dataReader["UserId"]);
                        rebuyDetail.TradeProfitRateId = Convert.ToInt32(dataReader["TradeProfitRateId"]);
                        rebuyDetail.SaleRate = Convert.ToDecimal(dataReader["SaleRate"]);
                        rebuyDetail.CurrencyPurchased = Convert.ToDecimal(dataReader["CurrencyPurchased"]);
                        rebuyDetail.MarketId = Convert.ToInt32(dataReader["MarketId"]);


                    }
                }
                return rebuyDetail;
            }
        }
        public async Task<List<ProfitLossStatisticsDetail>> GetUserProfitLossStatisticsGraph(int? UserId)
        {

            try
            {
                EnsureConnectionOpen();

                SqlParameter[] parameters =
                          {
                                     new SqlParameter("@inputUserId", SqlDbType.BigInt)
                          };
                parameters[0].Value = UserId;
                List<ProfitLossStatisticsDetail> profitLossStatisticsDetails = new List<ProfitLossStatisticsDetail>();
                using (var command = CreateCommand("usp_s_ProfitLossStatisticsGraph", CommandType.StoredProcedure, parameters))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            ProfitLossStatisticsDetail profitLossStatisticsDetail = new ProfitLossStatisticsDetail();
                            profitLossStatisticsDetail.MarketName = Convert.ToString(dataReader["MarketName"]);
                            profitLossStatisticsDetail.MarketId = Convert.ToInt32(dataReader["MarketId"]);
                            profitLossStatisticsDetail.TotalBoughtQuantity = Convert.ToDecimal(dataReader["TotalBoughtQuantity"]);
                            profitLossStatisticsDetail.AverageValue = Convert.ToDecimal(dataReader["AverageValue"]);
                            profitLossStatisticsDetail.TotalBuy = Convert.ToInt32(dataReader["TotalBuy"]);
                            profitLossStatisticsDetail.UserSessionId = Convert.ToInt32(dataReader["UserSessionId"]);
                            profitLossStatisticsDetail.IsSessionClosed = Convert.ToBoolean(dataReader["IsSessionClosed"]);
                            profitLossStatisticsDetail.UserId = Convert.ToInt32(dataReader["UserId"]);
                            profitLossStatisticsDetail.BTCAllocated = Convert.ToDecimal(dataReader["BTCAllocated"]);
                            profitLossStatisticsDetail.FirstBuyEquityPercentage = Convert.ToDecimal(dataReader["FirstBuyEquityPercentage"]);
                            profitLossStatisticsDetail.TradeProfitPercentage = Convert.ToDecimal(dataReader["TradeProfitPercentage"]);
                            profitLossStatisticsDetail.PurchaseOrderRecordId = Convert.ToInt32(dataReader["PurchaseOrderRecordId"]);
                            profitLossStatisticsDetail.SellAmount = Convert.ToDecimal(dataReader["SellAmount"]);
                            profitLossStatisticsDetail.SellQuantity = Convert.ToDecimal(dataReader["SellQuantity"]);
                            profitLossStatisticsDetail.SellRate = Convert.ToDecimal(dataReader["SellRate"]);
                            profitLossStatisticsDetail.BTCValueAfterTP = Convert.ToDecimal(dataReader["BTCValueAfterTP"]);
                            profitLossStatisticsDetail.TradePercentage = Convert.ToDecimal(dataReader["TradePercentage"]);
                            profitLossStatisticsDetail.BTCTradeProfit = Convert.ToDecimal(dataReader["BTCTradeProfit"]);
                            profitLossStatisticsDetail.TotalBTCAfterSell = Convert.ToDecimal(dataReader["TotalBTCAfterSell"]);
                            profitLossStatisticsDetail.MarketPercentage = Convert.ToDecimal(dataReader["MarketPercentage"]);


                            if (dataReader["IsSold"] != DBNull.Value)
                                profitLossStatisticsDetail.IsSold = Convert.ToBoolean(dataReader["IsSold"]);
                            else
                                profitLossStatisticsDetail.IsSold = false;

                            if (dataReader["IsAlreadySold"] != DBNull.Value)
                                profitLossStatisticsDetail.IsAlreadySold = Convert.ToBoolean(dataReader["IsAlreadySold"]);
                            else
                                profitLossStatisticsDetail.IsAlreadySold = false;
                            profitLossStatisticsDetail.SumBuyWith = Convert.ToDecimal(dataReader["SumBuyWith"]);
                            profitLossStatisticsDetails.Add(profitLossStatisticsDetail);
                        }
                       
                    }
                }
                return profitLossStatisticsDetails;
            }
            catch (Exception e)
            {

               return new List<ProfitLossStatisticsDetail>();
            }
        }

        public async Task<bool> TruncatePreserveInfo()
        {

            try
            {
                EnsureConnectionOpen();

                using (var command = CreateCommand("TRUNCATE TABLE PreserveBuyInfo", CommandType.Text))
                {
                    var result = await command.ExecuteNonQueryAsync();
                    return true;
                    //if (result != -1)
                    //{
                    //    return true;
                    //}
                    //else
                    //{
                    //    return false;
                    //}
                }

            }
            catch (Exception ex)
            {
                return false;
                //throw;
            }

        }

        public List<FirstBuyOrdersRecord> GetAllOpenOrders()
        {
            EnsureConnectionOpen();
            var result = new List<FirstBuyOrdersRecord>();
            using (var command = CreateCommand("usp_s_AllOpenOrders", CommandType.StoredProcedure))
            {
                using (var dataReader = command.ExecuteReader())
                {
                 
                    while (dataReader.Read())
                    {
                        FirstBuyOrdersRecord firstBuyOrdersRecord = new FirstBuyOrdersRecord();

                        firstBuyOrdersRecord.PurchaseOrderRecordId = Convert.ToInt32(dataReader["PurchaseOrderRecordId"]);
                        firstBuyOrdersRecord.CreatorUserId = Convert.ToInt32(dataReader["CreatorUserId"]);
                        firstBuyOrdersRecord.UserSessionDetailId = Convert.ToInt32(dataReader["UserSessionDetailId"]);
                        firstBuyOrdersRecord.OrderId = Convert.ToInt32(dataReader["OrderId"]);
                        firstBuyOrdersRecord.MarketName = Convert.ToString(dataReader["MarketName"]);
                        firstBuyOrdersRecord.MarketId = Convert.ToInt32(dataReader["MarketId"]);

                        if (dataReader["IsSold"] != DBNull.Value)
                            firstBuyOrdersRecord.IsSold = Convert.ToBoolean(dataReader["IsSold"]);
                        else
                            firstBuyOrdersRecord.IsSold = false;

                        if (dataReader["IsPendingOrder"] != DBNull.Value)
                            firstBuyOrdersRecord.IsPendingOrder = Convert.ToBoolean(dataReader["IsPendingOrder"]);
                        else
                            firstBuyOrdersRecord.IsPendingOrder = false;

                        if (dataReader["OrderType"] != DBNull.Value)
                            firstBuyOrdersRecord.OrderType = Convert.ToString(dataReader["OrderType"]);
                        else
                            firstBuyOrdersRecord.OrderType = "";


                        result.Add(firstBuyOrdersRecord);
                    }

                
                }
            }
            return result;
        }

        public  List<UserPackageExpiryResult> GetPackageExpiryDetails(bool? isTrial)
        {

            try
            {
                EnsureConnectionOpen();

                SqlParameter[] parameters =
                          {
                                     new SqlParameter("@IsTrial", SqlDbType.Bit)
                          };
                parameters[0].Value = isTrial;
                List<UserPackageExpiryResult> result = new List<UserPackageExpiryResult>();
                using (var command = CreateCommand("usp_s_UserPackageExpiryInfo", CommandType.StoredProcedure, parameters))
                {
                    using (var dataReader = command.ExecuteReader())
                    {

                       

                        while (dataReader.Read())
                        {
                            UserPackageExpiryResult packageExpiryResult = new UserPackageExpiryResult();
                            packageExpiryResult.UserName = Convert.ToString(dataReader["UserName"]);
                            packageExpiryResult.EmailAddress = Convert.ToString(dataReader["EmailAddress"]);
                            packageExpiryResult.RemainingDays = Convert.ToDecimal(dataReader["RemainingDays"]);
                            result.Add(packageExpiryResult);
                        }
                       
                    }
                }
                return result;
            }
            catch (Exception)
            {

                return new List<UserPackageExpiryResult>();
            }
        }

        public async Task<List<DashboardResult>> GetUserDashboardBuyUpDownDetail(int UserId)
        {

            try
            {
                EnsureConnectionOpen();

                SqlParameter[] parameters =
                          {
                                     new SqlParameter("@UserId", SqlDbType.BigInt)
                          };
                parameters[0].Value = UserId;
                List<DashboardResult> result = new List<DashboardResult>();
                using (var command = CreateCommand("usp_s_UserDashBoardBuyStopUpDown", CommandType.StoredProcedure, parameters))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            DashboardResult dashboardResult = new DashboardResult();
                            dashboardResult.MarketName = Convert.ToString(dataReader["MarketName"]);
                            dashboardResult.MarketId = Convert.ToInt32(dataReader["MarketId"]);                        
                            dashboardResult.AverageValue = Convert.ToDecimal(dataReader["BuyStopDownRate"]);
                            dashboardResult.NextBuyPoint = Convert.ToDecimal(dataReader["BuyStopUpRate"]);                           
                            dashboardResult.UserSessionId = Convert.ToInt32(dataReader["UserSessionId"]);
                            dashboardResult.IsSessionClosed = Convert.ToBoolean(dataReader["IsSessionClosed"]);
                            result.Add(dashboardResult);
                        }
                       
                    }
                }
                return result;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
