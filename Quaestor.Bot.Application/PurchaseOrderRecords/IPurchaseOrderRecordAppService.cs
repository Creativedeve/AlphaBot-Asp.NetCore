using Abp.Application.Services;
using Quaestor.Bot.PurchaseOrderRecords.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quaestor.Bot.PurchaseOrderRecords
{
    public interface IPurchaseOrderRecordAppService: IApplicationService
    {
        Task<PurchaseOrderRecordDto> Get(PurchaseOrderRecordSearchInput Input);
        Task<PurchaseOrderRecordDto> Create(CreatePurchaseOrderRecordInput input);
        Task<PurchaseOrderRecordDto> Update(EditPurchaseOrderRecordInput input);
       
    }
}
