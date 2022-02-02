using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Quaestor.Bot.Products.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quaestor.Bot.Products
{
    [AbpAuthorize]
    public class ProductAppService: BotAppServiceBase,IProductAppService
    {
        #region Properties
        private readonly IRepository<Product> _productsRepository;
        #endregion
        #region Constructor
        public ProductAppService(IRepository<Product> productsRepository)
        {
            _productsRepository = productsRepository;
        }
        #endregion

        #region Methods
        public ListResultDto<CreateProductDto> GetAllProducts()
        {
            var products = _productsRepository
                .GetAll()
                .ToList();

            return new ListResultDto<CreateProductDto>(ObjectMapper.Map<List<CreateProductDto>>(products));
        }
        public async Task<Product> GetProduct(ProductSearch input)
        {
            var products = await _productsRepository.GetAsync(input.Id);           
            return products;
        }

        public async Task CreateProduct(Product input)
        {
           
            try
            {
                var result = await _productsRepository.InsertAsync(input);
                await CurrentUnitOfWork.SaveChangesAsync();              
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
