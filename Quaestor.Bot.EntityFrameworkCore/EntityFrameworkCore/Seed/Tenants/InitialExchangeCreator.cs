using Quaestor.Bot.Exchanges;
using System;
using System.Linq;

namespace Quaestor.Bot.EntityFrameworkCore.Seed.Tenants
{
    public class InitialExchangeCreator
    {
        private readonly BotDbContext _context;

        public InitialExchangeCreator(BotDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            try
            {
                var binance = _context.Exchanges.FirstOrDefault(p => p.Name == "Binance");
                if (binance == null)
                {
                    _context.Exchanges.Add(
                        new Exchange
                        {
                            Name = "Binance"

                        });
                }
            }
            catch (Exception ex)
            {
                ;
            }
        }
    }
}
