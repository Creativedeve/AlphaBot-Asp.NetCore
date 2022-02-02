using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Quaestor.Bot.Products.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quaestor.Bot.Products
{
   public interface IProductAppService: IApplicationService
    {
        ListResultDto<CreateProductDto> GetAllProducts();
        Task<Product> GetProduct(ProductSearch input);
        Task CreateProduct(Product input);
    }
}
