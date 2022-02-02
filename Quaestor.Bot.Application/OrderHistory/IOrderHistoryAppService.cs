using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Quaestor.Bot.OrderHistory.Dto;
using Quaestor.Bot.PurchaseOrderRecords.Dto;

namespace Quaestor.Bot.OrderHistory
{
    public interface IOrderHistoryAppService : IApplicationService
    {        
        Task<List<Binance.Net.Objects.BinanceOrder>> GetUserOrderHistory(UserOrderHistoryInput Input);
    }
}
