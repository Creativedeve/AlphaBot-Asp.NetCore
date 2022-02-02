using Abp.Application.Services.Dto;

namespace Quaestor.Bot.PurchaseOrderRecords.Dto
{
    public class PurchaseOrderRecordSearchInput : EntityDto
    {

        public int? UserSessionId { get; set; }
    }

    public class PurchaseOrderRecordDetailSearchInput : EntityDto
    {

        public int? PurchaseOrderRecordId { get; set; }
    }
}
