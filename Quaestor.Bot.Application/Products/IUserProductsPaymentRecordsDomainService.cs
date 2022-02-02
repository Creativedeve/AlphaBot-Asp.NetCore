using Abp.Application.Services.Dto;
using Abp.Domain.Services;
using Quaestor.Bot.Products.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quaestor.Bot.Products
{
    public interface IUserProductsPaymentRecordsDomainService: IDomainService
    {
        Task CreateUserProudctPayment(CreateUserProductsPaymentRecordsDto input);
        Task UpdateUserProudctPayment(UserProductsPaymentRecords input);
        UserProductsPaymentRecords GetPaymentByCode(UserProductsPaymentRecordSearch input);
        ListResultDto<UserProductsPaymentRecords> GetUserProductsPaymentsByUserID(UserProductSearch input);
    }
}
