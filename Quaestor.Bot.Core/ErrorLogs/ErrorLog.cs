using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quaestor.Bot.ErrorLogs
{
    [Table("ErrorLogs")]
    public class ErrorLog : FullAuditedEntity
    {
        public string MarketName { get; set; }
        public string Status { get; set; }
        public string ExchangeName { get; set; }
        public long OrderId { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorType { get; set; }
        public int UserId { get; set; }   
        public  int UserSessionDetailId { get; set; }
        public int PurchaseOrderDetailId { get; set; }
    }
}
