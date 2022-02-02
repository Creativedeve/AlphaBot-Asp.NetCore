using Abp.Application.Services;
using Quaestor.Bot.ErrorLogs.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quaestor.Bot.ErrorLogs
{
   public interface IErrorLogAppService : IApplicationService
    {
        Task<ErrorLog> CreateErrorLog(ErrorLogCreateInput input);
    }
}
