using Abp.Application.Services.Dto;

namespace Quaestor.Bot.PurchaseOrderRecords.Dto
{
    public class EditPurchasedOrderRecordDetailInput : EntityDto<int>
    {
        
        public int ReBuySequence { get; set; }
        public decimal? ReBuyRate { get; set; }
        public decimal? ReBuyValue { get; set; }
        public decimal? ReBuyWith { get; set; }
        public int PurchaseOrderRecordId { get; set; }
    }
}
