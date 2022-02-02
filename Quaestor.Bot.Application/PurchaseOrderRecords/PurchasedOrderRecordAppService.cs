using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;
using Quaestor.Bot.PurchaseOrderRecords.Dto;
using System;
using System.Threading.Tasks;

namespace Quaestor.Bot.PurchaseOrderRecords
{
    public class PurchasedOrderRecordAppService : AsyncCrudAppService<PurchaseOrderRecord, PurchaseOrderRecordDto, int, PurchaseOrderRecordSearchInput, CreatePurchaseOrderRecordInput, EditPurchaseOrderRecordInput, PurchaseOrderRecordSearchInput>, IPurchaseOrderRecordAppService
    {
        #region Properties
        private readonly IRepository<PurchaseOrderRecord, int> _purchasedOrderRecordRepository;
        #endregion

        #region Constructor
        public PurchasedOrderRecordAppService(IRepository<PurchaseOrderRecord, int> repository)
          : base(repository)
        {
            _purchasedOrderRecordRepository = repository;
        }
        #endregion

        #region Methods
        public override async Task<PurchaseOrderRecordDto> Create(CreatePurchaseOrderRecordInput input)
        {
            try
            {
                var purchaseOrder = ObjectMapper.Map<PurchaseOrderRecord>(input);
                await _purchasedOrderRecordRepository.InsertAsync(purchaseOrder); ;
                await CurrentUnitOfWork.SaveChangesAsync();
                var Order = ObjectMapper.Map<PurchaseOrderRecordDto>(purchaseOrder);
                return Order;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public override async Task<PurchaseOrderRecordDto> Update(EditPurchaseOrderRecordInput input)
        {
            try
            {
                var purchaseOrder = await _purchasedOrderRecordRepository.GetAsync(input.Id);
                MapToEntity(input, purchaseOrder);
                await _purchasedOrderRecordRepository.UpdateAsync(purchaseOrder);
                await CurrentUnitOfWork.SaveChangesAsync();
                var result = await _purchasedOrderRecordRepository.GetAsync(input.Id);
                return ObjectMapper.Map<PurchaseOrderRecordDto>(result);
            }
            catch (Exception ex)
            {
                return null;
            }


        }
        public override async Task<PurchaseOrderRecordDto> Get(PurchaseOrderRecordSearchInput input)
        {
            try
            {
                var purchaseOrder = await _purchasedOrderRecordRepository.GetAsync(input.Id);
                return ObjectMapper.Map<PurchaseOrderRecordDto>(purchaseOrder);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion
    }
}
