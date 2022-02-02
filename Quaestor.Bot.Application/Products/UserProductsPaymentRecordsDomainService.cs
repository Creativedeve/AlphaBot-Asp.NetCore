using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Quaestor.Bot.Products.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quaestor.Bot.Products
{
    public class UserProductsPaymentRecordsDomainService : DomainService, IUserProductsPaymentRecordsDomainService
    {
        #region Properties
        private readonly IRepository<UserProductsPaymentRecords> _userProductsPaymentRecordsRepository;
        #endregion
        #region Constructor
        public UserProductsPaymentRecordsDomainService(IRepository<UserProductsPaymentRecords> userProductsPaymentRecordsRepository)
        {
            _userProductsPaymentRecordsRepository = userProductsPaymentRecordsRepository;
        }

        #endregion

        #region Methods

        public async Task CreateUserProudctPayment(CreateUserProductsPaymentRecordsDto input)
        {
            try
            {
                var userProudctPayment = ObjectMapper.Map<UserProductsPaymentRecords>(input);
               var result = await _userProductsPaymentRecordsRepository.InsertAsync(userProudctPayment);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }    
          
        }

        public UserProductsPaymentRecords GetPaymentByCode(UserProductsPaymentRecordSearch input)
        {
            var payment =  _userProductsPaymentRecordsRepository.GetAll().Where(x => x.Code == input.Code).FirstOrDefault();
            return payment;
        }

        public async Task UpdateUserProudctPayment(UserProductsPaymentRecords input)
        {
            try
            {
                await _userProductsPaymentRecordsRepository.UpdateAsync(input);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }        
          
        }
        public ListResultDto<UserProductsPaymentRecords> GetUserProductsPaymentsByUserID(UserProductSearch input)
        {
            var payment = _userProductsPaymentRecordsRepository.GetAll().Where(x => x.CreatorUserId == input.UserId).ToList();
            return new ListResultDto<UserProductsPaymentRecords>(ObjectMapper.Map<List<UserProductsPaymentRecords>>(payment));
        }
        #endregion
    }
}
