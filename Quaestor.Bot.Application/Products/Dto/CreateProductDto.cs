using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quaestor.Bot.Products.Dto
{

    [AutoMapTo(typeof(Product))]
    public  class CreateProductDto: EntityDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
    }
    public class ProductSearch
    {
        public int Id { get; set; }
    }
}
