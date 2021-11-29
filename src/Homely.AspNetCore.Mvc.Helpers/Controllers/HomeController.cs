using Homely.AspNetCore.Mvc.Helpers.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Homely.AspNetCore.Mvc.Helpers.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeControllerBanner _banner;

        public HomeController(IHomeControllerBanner banner)
        {
            _banner = banner;
        }

        /// <summary>
        /// Main Api home page. Just  means the API is up and running.
        /// </summary>
        /// <returns>Some health-related text.</returns>
        /// <response code="200">Some text saying "yep, we're up!".</response>
        [HttpGet("", Name = "GET_Home")]
        public OkObjectResult Get()
        {
            return Ok(_banner.Banner);
        }

        [HttpGet("exceptionTest", Name = "GET_ExceptionTest")]
        [ProducesResponseType(500)]
        public ActionResult ExceptionTest()
        {
            throw new Exception($"Testing exceptions. {DateTime.UtcNow}");
        }
    }
}
