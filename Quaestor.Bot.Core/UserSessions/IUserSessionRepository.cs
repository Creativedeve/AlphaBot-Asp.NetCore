using Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quaestor.Bot.DashBoard;
using System;
using Quaestor.Bot.PackagesDetail;

namespace Quaestor.Bot.UserSessions
{
    public interface IUserSessionRepository : IRepository<UserSession>
    {
        List<RebuyDetail> GetAllRebuy();
        List<FirstBuyDetail> GetAllFirstBuy(int? userId);
        List<TradeProfitDetailResult> GetAllTradeProfitRecords();
        Task<List<DashboardResult>> GetUserDashboardDetail(int? UserId);

        List<BuyStopUpDownResult> GetAllBuyStopUpAndDown();

        bool IsAlreadyBought(string BuyType, int MarketId, int? UserSessionDetailId = null, int? ReBuySequence = null, int? PurchaseOrderRecordId = null);
        TradeProfitDetailResult SellAllBoughtRecordsAgainstUser(Int64 UserId,int PurchaseOrderRecordId, int MarketId);

        Task<List<ProfitLossStatisticsDetail>> GetUserProfitLossStatisticsGraph(int? UserId);
        Task<bool> TruncatePreserveInfo();
        List<FirstBuyOrdersRecord> GetAllOpenOrders();
        List<UserPackageExpiryResult> GetPackageExpiryDetails(bool? isTrial);

        Task<List<DashboardResult>> GetUserDashboardBuyUpDownDetail(int UserId);
    }
}
