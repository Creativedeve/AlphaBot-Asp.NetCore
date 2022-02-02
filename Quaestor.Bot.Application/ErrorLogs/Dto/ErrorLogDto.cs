using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quaestor.Bot.ErrorLogs.Dto
{
    [AutoMapTo(typeof(ErrorLog))]
    public class ErrorLogDto:EntityDto<int>
    {
        public string MarketName { get; set; }

        public string Status { get; set; }

        public string ExchangeName { get; set; }

        public int OrderId { get; set; }
    }
}
