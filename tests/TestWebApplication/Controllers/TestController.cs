using Homely.AspNetCore.Mvc.Helpers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestWebApplication.Models;
using TestWebApplication.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestWebApplication.Controllers
{
    [AllowAnonymous]
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IFakeVehicleRepository _fakeVehicleRepository;

        public TestController(IFakeVehicleRepository fakeVehicleRepository)
        {
            _fakeVehicleRepository = fakeVehicleRepository ?? throw new ArgumentNullException(nameof(fakeVehicleRepository));
        }

        // GET: /test/1 | 200 OK.
        [HttpGet("{id:int}", Name ="GetId")]
        public IActionResult Get(int id)
        {
            var model = _fakeVehicleRepository.Get(id);

            return model == null
                ? (IActionResult)NotFound()
                : Ok(model);
        }

        // GET: /test/notFound | 404 Not Found.
        [HttpGet("notfound")]
        public IActionResult GetNotFound()
        {
            return NotFound();
        }

        // POST: /test | 201 Created.
        [HttpPost]
        public IActionResult Post(FakeVehicle fakeVehicle)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Model is invalid -> it should have been auto checked and auto 400 returned.");
            }

            _fakeVehicleRepository.Add(fakeVehicle);

            return CreatedAtRoute("GetId", new { id = fakeVehicle.Id }, null);
        }

        // GET: /test/error | 500 Server Error.
        [HttpGet("error")]
        public IActionResult Error()
        {
            throw new Exception("Something bad ass happened.");
        }

        // GET: /test/dynamicError | 500 Server Error.
        // This tests that an exception HTTP STATUS 500 doesn't get cached. 
        [HttpGet("dynamicError")]
        public IActionResult DynamicError()
        {
            throw new Exception(Guid.NewGuid().ToString());
        }

        // Specific Status Code check | 409 Conflict.
        [HttpGet("conflict")]
        public IActionResult ConflictCheck()
        {
            return StatusCode(409, new ApiErrorResult("agent was already modified"));
        }

        [HttpGet("slowDelay")]
        [HttpGet("slowDelay/{seconds:int}")]
        public async Task<IActionResult> SlowDelay(CancellationToken cancellationToken, int seconds = 50)
        {
            // Pretend some logic takes a long time. Like getting some data from a database.
            var startTime = DateTime.UtcNow;

            await Task.Delay(1000 * seconds, cancellationToken);

            var endTime = DateTime.UtcNow;

            return Ok(new
            {
                startTime,
                endTime
            });
        }
    }
}
