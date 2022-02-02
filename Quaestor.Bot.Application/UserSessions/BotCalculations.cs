using Quaestor.Bot.PurchaseOrderRecords;
using Quaestor.Bot.PurchaseOrderRecords.Dto;
using Quaestor.Bot.TradeProfitRates;
using Quaestor.Bot.UserSessions.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quaestor.Bot.UserSessions
{

    public class BotCalculations
    {
        private UserSessionDetail mObj_sessionDetail = null;
        private PurchaseOrderRecord mObj_PurchaseOrderRecord = null;
        private List<CreatePurchaseOrderRecordDetailInput> mLst_PurchaseOrderRecordDetail = null;
        private PurchasedOrderRecordAppService mObj_PurchasedOrderRecordAppService = null;
        private CreatePurchaseOrderRecordInput mObj_CreatePurchaseOrderRecordInput = null;
        private TradeProfitRate mObj_TradeProfitRate = null;
        public BotCalculations()
        {


        }       

        public UserSessionDetail BotSessionDetail
        {
            get
            {
                return mObj_sessionDetail;
            }
            set
            {
                mObj_sessionDetail = value;
            }
        }

        public PurchaseOrderRecord BotPurchaseOrderRecord
        {
            get
            {
                return mObj_PurchaseOrderRecord;
            }
            set
            {
                mObj_PurchaseOrderRecord = value;
            }
        }

        public List<CreatePurchaseOrderRecordDetailInput> BotPurchaseOrderRecordDetail
        {
            get
            {
                return mLst_PurchaseOrderRecordDetail;
            }
            set
            {
                mLst_PurchaseOrderRecordDetail = value;
            }
        }

        public PurchasedOrderRecordAppService BotPurchasedOrderRecordDetailAppService
        {
            set
            {
                mObj_PurchasedOrderRecordAppService = value;
            }
        }

        public TradeProfitRate BotTradeProfitRate
        {
            get
            {
                return mObj_TradeProfitRate;
            }
        }

        public async Task<bool> GetFirstBuy()
        {

            try
            {
                if (mObj_sessionDetail is null)
                {
                    return false;
                }

                mObj_PurchaseOrderRecord.FirstBuyWith = mObj_sessionDetail.BTCAllocated * mObj_sessionDetail.FirstBuyEquityPercentage / 100;



                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<bool> SetReBuyRateValuePair()
        {
            CreatePurchaseOrderRecordDetailInput lObj_PurchaseOrderRecordDetail = null;
            decimal? purchaseRate = 0;
            decimal? allocatedBalance = 0;

            try
            {
                if (mObj_sessionDetail is null)
                {
                    return false;
                }

                if (mObj_PurchasedOrderRecordAppService is null)
                {
                    return false;
                }

                if (mLst_PurchaseOrderRecordDetail is null)
                {
                    mLst_PurchaseOrderRecordDetail = new List<CreatePurchaseOrderRecordDetailInput>();
                }

                purchaseRate = mObj_PurchaseOrderRecord.FirstBuyWith;
                allocatedBalance = mObj_sessionDetail.BTCAllocated;

                lObj_PurchaseOrderRecordDetail = new CreatePurchaseOrderRecordDetailInput();
                lObj_PurchaseOrderRecordDetail.PurchaseOrderRecordId = mObj_PurchaseOrderRecord.Id;
                lObj_PurchaseOrderRecordDetail.ReBuySequence = 1;
                lObj_PurchaseOrderRecordDetail.ReBuyRate = allocatedBalance * mObj_sessionDetail.FirstRebuy / 100;
                lObj_PurchaseOrderRecordDetail.ReBuyWith = purchaseRate - (purchaseRate * mObj_sessionDetail.FirstRebuyDrop / 100);
                mLst_PurchaseOrderRecordDetail.Add(lObj_PurchaseOrderRecordDetail);

                lObj_PurchaseOrderRecordDetail = new CreatePurchaseOrderRecordDetailInput();
                lObj_PurchaseOrderRecordDetail.PurchaseOrderRecordId = mObj_PurchaseOrderRecord.Id;
                lObj_PurchaseOrderRecordDetail.ReBuySequence = 2;
                lObj_PurchaseOrderRecordDetail.ReBuyRate = allocatedBalance * mObj_sessionDetail.SecondRebuy / 100;
                lObj_PurchaseOrderRecordDetail.ReBuyWith = purchaseRate - (purchaseRate * mObj_sessionDetail.SecondRebuyDrop / 100);
                mLst_PurchaseOrderRecordDetail.Add(lObj_PurchaseOrderRecordDetail);

                lObj_PurchaseOrderRecordDetail = new CreatePurchaseOrderRecordDetailInput();
                lObj_PurchaseOrderRecordDetail.PurchaseOrderRecordId = mObj_PurchaseOrderRecord.Id;
                lObj_PurchaseOrderRecordDetail.ReBuySequence = 3;
                lObj_PurchaseOrderRecordDetail.ReBuyRate = allocatedBalance * mObj_sessionDetail.ThirdRebuy / 100;
                lObj_PurchaseOrderRecordDetail.ReBuyWith = purchaseRate - (purchaseRate * mObj_sessionDetail.ThirdRebuyDrop / 100);
                mLst_PurchaseOrderRecordDetail.Add(lObj_PurchaseOrderRecordDetail);

                lObj_PurchaseOrderRecordDetail = new CreatePurchaseOrderRecordDetailInput();
                lObj_PurchaseOrderRecordDetail.PurchaseOrderRecordId = mObj_PurchaseOrderRecord.Id;
                lObj_PurchaseOrderRecordDetail.ReBuySequence = 4;
                lObj_PurchaseOrderRecordDetail.ReBuyRate = allocatedBalance * mObj_sessionDetail.FourthRebuy / 100;
                lObj_PurchaseOrderRecordDetail.ReBuyWith = purchaseRate - (purchaseRate * mObj_sessionDetail.FourthRebuyDrop / 100);
                mLst_PurchaseOrderRecordDetail.Add(lObj_PurchaseOrderRecordDetail);

                lObj_PurchaseOrderRecordDetail = new CreatePurchaseOrderRecordDetailInput();
                lObj_PurchaseOrderRecordDetail.PurchaseOrderRecordId = mObj_PurchaseOrderRecord.Id;
                lObj_PurchaseOrderRecordDetail.ReBuySequence = 5;
                lObj_PurchaseOrderRecordDetail.ReBuyRate = allocatedBalance * mObj_sessionDetail.FifthRebuy / 100;
                lObj_PurchaseOrderRecordDetail.ReBuyWith = purchaseRate - (purchaseRate * mObj_sessionDetail.FifthRebuyDrop / 100);
                mLst_PurchaseOrderRecordDetail.Add(lObj_PurchaseOrderRecordDetail);

                lObj_PurchaseOrderRecordDetail = new CreatePurchaseOrderRecordDetailInput();
                lObj_PurchaseOrderRecordDetail.PurchaseOrderRecordId = mObj_PurchaseOrderRecord.Id;
                lObj_PurchaseOrderRecordDetail.ReBuySequence = 6;
                lObj_PurchaseOrderRecordDetail.ReBuyRate = allocatedBalance * mObj_sessionDetail.SixthRebuy / 100;
                lObj_PurchaseOrderRecordDetail.ReBuyWith = purchaseRate - (purchaseRate * mObj_sessionDetail.SixthRebuyDrop / 100);
                mLst_PurchaseOrderRecordDetail.Add(lObj_PurchaseOrderRecordDetail);

                lObj_PurchaseOrderRecordDetail = new CreatePurchaseOrderRecordDetailInput();
                lObj_PurchaseOrderRecordDetail.PurchaseOrderRecordId = mObj_PurchaseOrderRecord.Id;
                lObj_PurchaseOrderRecordDetail.ReBuySequence = 7;
                lObj_PurchaseOrderRecordDetail.ReBuyRate = allocatedBalance * mObj_sessionDetail.SeventRebuy / 100;
                lObj_PurchaseOrderRecordDetail.ReBuyWith = purchaseRate - (purchaseRate * mObj_sessionDetail.SeventhRebuyDrop / 100);
                mLst_PurchaseOrderRecordDetail.Add(lObj_PurchaseOrderRecordDetail);

                mObj_CreatePurchaseOrderRecordInput.PurchaseOrderRecordDetails = mLst_PurchaseOrderRecordDetail;
                await mObj_PurchasedOrderRecordAppService.Create(mObj_CreatePurchaseOrderRecordInput);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool GetTradeProfitRate()
        {
            try
            {
                if (mObj_TradeProfitRate is null)
                {
                    mObj_TradeProfitRate = new TradeProfitRate();
                }

                mObj_TradeProfitRate.BTCInvested = mObj_PurchaseOrderRecord.FirstBuyWith;
                mObj_TradeProfitRate.CurrencyPurchased = mObj_PurchaseOrderRecord.FirstBuyValue;

                foreach (CreatePurchaseOrderRecordDetailInput lObj_PurchaseOrderRecordDetail in mLst_PurchaseOrderRecordDetail)
                {
                    mObj_TradeProfitRate.BTCInvested += lObj_PurchaseOrderRecordDetail.ReBuyWith;
                    mObj_TradeProfitRate.CurrencyPurchased += lObj_PurchaseOrderRecordDetail.ReBuyValue;
                }

                mObj_TradeProfitRate.AverageCurrencyRate = mObj_TradeProfitRate.BTCInvested / mObj_TradeProfitRate.CurrencyPurchased;
                mObj_TradeProfitRate.TradeProfitPercentageRate = mObj_TradeProfitRate.AverageCurrencyRate * mObj_sessionDetail.TradeProfitPercentage / 100;
                mObj_TradeProfitRate.TradeProfitSaleRate = mObj_TradeProfitRate.AverageCurrencyRate + mObj_TradeProfitRate.TradeProfitPercentageRate;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
