using Quaestor.Bot.DashBoard;
using Quaestor.Bot.UserKeys.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quaestor.Bot.ViewModels
{
    public class DashboardDetail
    {
        public BalanceInfo BalanceInfo { get; set; }
        public List<Binance.Net.Objects.BinanceBalance> BinanceBalances { get; set; }
        public List<DashboardResult> DashboardResults { get; set; }
        public List<ProfitLossStatisticsDetail> ProfitLossStatisticsDetails { get; set; }
    }
}
