using Abp.Application.Services.Dto;
using System;

namespace Quaestor.Bot.Exchanges.Dto
{
    public class ExchangeListDto : EntityDto
    {
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
