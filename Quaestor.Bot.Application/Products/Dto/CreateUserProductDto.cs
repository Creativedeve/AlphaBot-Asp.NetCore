using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quaestor.Bot.Products.Dto
{

    [AutoMapTo(typeof(UserProducts))]
    public class CreateUserProductDto : EntityDto
    {
       // public virtual Product ProductDetail { get; set; }
        public int ProductId { get; set; }
        public int CreatorUserId { get; set; }
    }
    public class UserProductSearch
    {
        public int UserId { get; set; }
        public int Id { get; set; }
    }
}
