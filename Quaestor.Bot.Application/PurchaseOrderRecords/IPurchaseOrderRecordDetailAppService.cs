using Abp.Application.Services;
using Quaestor.Bot.PurchaseOrderRecords.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quaestor.Bot.PurchaseOrderRecords
{
    public interface IPurchaseOrderRecordDetailAppService:IApplicationService 
    {
        Task<PurchaseOrderRecordDetailDto> Create(CreatePurchaseOrderRecordDetailInput input);
        Task<PurchaseOrderRecordDetailDto> Update(EditPurchasedOrderRecordDetailInput input);
        Task<PurchaseOrderRecordDetailDto> GetById(PurchaseOrderRecordDetailSearchInput input);
        Task Delete(DeleteInput input);
    }
}
