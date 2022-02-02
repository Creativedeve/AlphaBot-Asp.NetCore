using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Quaestor.Bot.Products.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quaestor.Bot.Products
{
    public interface IUserProductAppService: IApplicationService
    {
        Task<string> CreateUserProduct(CreateUserProductDto input);
        ListResultDto<UserProducts> GetUserProductByUserID(UserProductSearch input);
        Task<UserProducts> GetUserProductByID(UserProductSearch input);
    }
}
