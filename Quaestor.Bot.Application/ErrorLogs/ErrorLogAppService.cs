using Abp.Application.Services;
using Abp.Domain.Repositories;
using Quaestor.Bot.ErrorLogs.Dto;
using System;
using System.Threading.Tasks;

namespace Quaestor.Bot.ErrorLogs
{
    public class ErrorLogAppService : AsyncCrudAppService<ErrorLog, ErrorLogDto, int, ErrorLogCreateInput, ErrorLogCreateInput, ErrorLogCreateInput, ErrorLogCreateInput, ErrorLogCreateInput>, IErrorLogAppService
    {
        #region Properties
        private readonly IRepository<ErrorLog> _errorLogRepository;
        #endregion

        #region Constructor
        public ErrorLogAppService(IRepository<ErrorLog> repository) : base(repository)
        {
            _errorLogRepository = repository;
        }
        #endregion

        #region Methods
        public async Task<ErrorLog> CreateErrorLog(ErrorLogCreateInput input)
        {
            try
            {                
                //var errorLog1 = ObjectMapper.Map<ErrorLog>(input);

                ErrorLog errorLog = new ErrorLog();
                errorLog.Status = input.Status;
                errorLog.ExchangeName = input.ExchangeName;
                errorLog.OrderId = input.OrderId;
                errorLog.IsDeleted = false;
                errorLog.CreationTime = DateTime.Now;
                errorLog.CreatorUserId = input.CreatorUserId;
                errorLog.Id = 0;
                errorLog.MarketName = input.MarketName;
                


               var result= await _errorLogRepository.InsertAsync(errorLog);
                return result;
            }
            catch (Exception ex)
            {
                await Task.FromResult(0);
                return null;
            }

        }
        public override Task<ErrorLogDto> Create(ErrorLogCreateInput input)
        {
            return base.Create(input);
        }
        #endregion
    }
}
