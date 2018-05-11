using Microsoft.AspNetCore.Mvc;
using System;

namespace TestWebApplication.Controllers
{
    // NOTE: By default, this will be auto [Authorize]'d because of the global filter.
    [Route("Authorized")]
    public class AuthorizedController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return Ok($"Woot! Congrats, you're authorized. Current DateTime: {DateTime.Now}. IsAuthenticated: {User.Identity.IsAuthenticated}");
        }
    }
}
