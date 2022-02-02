using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CoinbaseCommerce;
using Microsoft.AspNetCore.Identity;
using Quaestor.Bot.Authorization.Users;
using Quaestor.Bot.Products.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quaestor.Bot.Products
{
    [AbpAuthorize]
    public class UserProductAppService : BotAppServiceBase, IUserProductAppService
    {
        #region Properties
        private readonly IRepository<UserProducts> _userProductsRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly UserManager _userManager;
        #endregion
        #region Constructor
        public UserProductAppService(IRepository<UserProducts> userProductsRepository, IRepository<Product> productRepository, UserManager userManager)
        {
            _userProductsRepository = userProductsRepository;
            _productRepository = productRepository;
            _userManager = userManager;
        }
        #endregion

        #region Methods
        public async Task<string> CreateUserProduct(CreateUserProductDto input)
        {
            string responseURL = string.Empty;
            try
            {

                var userProudct = ObjectMapper.Map<UserProducts>(input);                
                var result = await _userProductsRepository.InsertAsync(userProudct);
                await CurrentUnitOfWork.SaveChangesAsync();
                var username = _userManager.Users.Where(x => x.Id == result.CreatorUserId).FirstOrDefault().UserName;
                result.ProductDetail = new Product();
                result.ProductDetail = _productRepository.Get(result.ProductId);
                var chargeResult = new CoinbaseImp().PrepareCharge(username, Convert.ToInt32(result.CreatorUserId), result.ProductId, result.ProductDetail.Description, "EUR", result.ProductDetail.Price, result.Id);
                return responseURL = chargeResult.Result;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ListResultDto<UserProducts> GetUserProductByUserID(UserProductSearch input)
        {
            var products = _userProductsRepository
             .GetAll().Where(x=>x.CreatorUserId==input.UserId)
             .ToList();

            return new ListResultDto<UserProducts>(ObjectMapper.Map<List<UserProducts>>(products));
        }
        public async Task<UserProducts> GetUserProductByID(UserProductSearch input)
        {
            var products = await _userProductsRepository.GetAsync(input.Id);
            return products;
        }
        #endregion
    }
}

