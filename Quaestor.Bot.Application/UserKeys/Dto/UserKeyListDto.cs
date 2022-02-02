using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quaestor.Bot.UserKeys.Dto
{
   public class UserKeyListDto: EntityDto
    {
        public string SecretKey { get; set; }
        public string ApiKey { get; set; }
        public int ExchangeId { get; set; }
        public virtual long UserId { get; set; }
    }
}
