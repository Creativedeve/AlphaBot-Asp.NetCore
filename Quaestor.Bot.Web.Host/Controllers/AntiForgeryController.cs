using Microsoft.AspNetCore.Antiforgery;
using Quaestor.Bot.Controllers;

namespace Quaestor.Bot.Web.Host.Controllers
{
    public class AntiForgeryController : BotControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
