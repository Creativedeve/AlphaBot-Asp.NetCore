using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace Quaestor.Bot.ErrorLogs.Dto
{
    [AutoMapTo(typeof(ErrorLog))]
    public class ErrorLogCreateInput:EntityDto
    {
        public string MarketName { get; set; }

        public string Status { get; set; }

        public string ExchangeName { get; set; }

        public long OrderId { get; set; }
        public long CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
