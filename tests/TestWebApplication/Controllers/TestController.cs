using Hellang.Middleware.ProblemDetails;
using Homely.AspNetCore.Mvc.Helpers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public IActionResult Post([FromForm] FakeVehicle fakeVehicle)
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

        // GET: /test/modelbindingerror | 400 Bad Request. (Model Binding failure check)
        [HttpGet("modelbinding/{colour}")]
        public IActionResult ModelBindingTest(ColourType colour = ColourType.Unknown)
        {
            return Ok(colour);
        }

        // GET: /test/validationerror | 400 Bad Request. (Manual validation check)
        [HttpGet("validationerror")]
        public IActionResult ValidationError()
        {
            ModelState.AddModelError("someProperty", "This property failed validation.");

            var validation = new ValidationProblemDetails(ModelState)
            {
                Type = "https://httpstatuses.com/400",
                Status = StatusCodes.Status400BadRequest
            };

            throw new ProblemDetailsException(validation);
        }

        // Specific Status Code check | 409 Conflict.
        [HttpGet("conflict")]
        public IActionResult ConflictCheck()
        {
            var error = new ProblemDetails
            {
                Type = "https://httpstatuses.com/409",
                Title = "Agent was already modified.",
                Status = StatusCodes.Status409Conflict,
                Instance = "/test/conflict",
                Detail = "agent was already modified after you retrieved the latest data. So you would then override the most recent copy. As such, you will need to refresh the page (to get the latest data) then modify that, if required."
            };

            return StatusCode(409, error);
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
