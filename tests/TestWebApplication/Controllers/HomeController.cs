using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace TestWebApplication.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return Ok($"Current DateTime: {DateTime.Now}");
        }
    }
}
