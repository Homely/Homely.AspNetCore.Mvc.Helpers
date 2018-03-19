using Microsoft.AspNetCore.Mvc;
using System;

namespace TestWebApplication.Controllers
{
    
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return Ok($"Current DateTime: {DateTime.Now}");
        }
    }
}
