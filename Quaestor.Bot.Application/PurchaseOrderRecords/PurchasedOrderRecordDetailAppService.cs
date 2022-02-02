using Abp.Application.Services;
using Abp.Domain.Repositories;
using Quaestor.Bot.PurchaseOrderRecords.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quaestor.Bot.PurchaseOrderRecords
{
    public class PurchasedOrderRecordDetailAppService : AsyncCrudAppService<PurchaseOrderRecordDetail, PurchaseOrderRecordDetailDto, int, PurchaseOrderRecordSearchInput, CreatePurchaseOrderRecordDetailInput, EditPurchasedOrderRecordDetailInput, PurchaseOrderRecordSearchInput, DeleteInput>, IPurchaseOrderRecordDetailAppService
    {
        #region Properties
        private readonly IRepository<PurchaseOrderRecordDetail, int> _purchasedOrderRecordDetailRepository;
        #endregion

        #region Constructor
        public PurchasedOrderRecordDetailAppService(IRepository<PurchaseOrderRecordDetail, int> repository)
          : base(repository)
        {
            _purchasedOrderRecordDetailRepository = repository;
        }
        #endregion

        #region Methods
        public override async Task<PurchaseOrderRecordDetailDto> Create(CreatePurchaseOrderRecordDetailInput input)
        {
            try
            {
                var purchaseOrder = ObjectMapper.Map<PurchaseOrderRecordDetail>(input);
                await _purchasedOrderRecordDetailRepository.InsertAsync(purchaseOrder); ;
                await CurrentUnitOfWork.SaveChangesAsync();
                var Order = ObjectMapper.Map<PurchaseOrderRecordDetailDto>(purchaseOrder);
                return Order;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public override async Task<PurchaseOrderRecordDetailDto> Update(EditPurchasedOrderRecordDetailInput input)
        {
            try
            {
                var purchaseOrderDetail = await _purchasedOrderRecordDetailRepository.GetAsync(input.Id);
                MapToEntity(input, purchaseOrderDetail);
                await _purchasedOrderRecordDetailRepository.UpdateAsync(purchaseOrderDetail);
                await CurrentUnitOfWork.SaveChangesAsync();
                var result = await _purchasedOrderRecordDetailRepository.GetAsync(input.Id);
                return ObjectMapper.Map<PurchaseOrderRecordDetailDto>(result);
            }
            catch (Exception ex)
            {
                return null;
            }


        }
        public async Task<PurchaseOrderRecordDetailDto> GetById(PurchaseOrderRecordDetailSearchInput input)
        {
            try
            {
                var purchaseOrderDetail = await _purchasedOrderRecordDetailRepository.GetAsync(input.Id);
                return ObjectMapper.Map<PurchaseOrderRecordDetailDto>(purchaseOrderDetail);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public override Task Delete(DeleteInput input)
        {
            return base.Delete(input);
        }
        #endregion
    }
}
