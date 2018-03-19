using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TestWebApplication.Models;
using TestWebApplication.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestWebApplication.Controllers
{
    [Route("test")]
    public class TestController : Controller
    {
        private readonly IFakeVehicleRepository _fakeVehicleRepository;

        public TestController(IFakeVehicleRepository fakeVehicleRepository)
        {
            _fakeVehicleRepository = fakeVehicleRepository ?? throw new ArgumentNullException(nameof(fakeVehicleRepository));
        }
        
        // GET: /test/1
        [HttpGet("{id:int}", Name ="GetId")]
        public IActionResult Get(int id)
        {
            var model = _fakeVehicleRepository.Get(id);

            return model == null 
                ? (IActionResult)NotFound() 
                : Ok(model);
        }

        // GET: /test/notFound
        [HttpGet("/notfound")]
        public IActionResult GetNotFound()
        {
            return NotFound();
        }

        // POST: /test
        [HttpPost]
        public IActionResult Post(FakeVehicle fakeVehicle)
        {
            _fakeVehicleRepository.Add(fakeVehicle);

            return CreatedAtRoute("GetId", new { id = fakeVehicle.Id }, null);
        }

        // GET: /test/error
        [HttpGet("error")]
        public IActionResult Error()
        {
            throw new Exception("Something bad ass happened.");
        }

        // GET: /test/dynamicError
        // This tests that an exception HTTP STATUS 500 doesn't get cached. 
        [HttpGet("dynamicError")]
        public IActionResult DynamicError()
        {
            throw new Exception(Guid.NewGuid().ToString());
        }

        // GET: /test/validationerror
        [HttpGet("validationError/{id?}")]
        public IActionResult ValidationError(int id = 1)
        {
            var errors = new List<ValidationFailure>
            {
                new ValidationFailure("age", "Age is not valid."),
                new ValidationFailure("id", "no person Id was provided."),
                new ValidationFailure("name", "No person name was provided.")
            };
            
            throw new ValidationException(errors);
        }

        // GET: /test/dynamicValidationerror
        [HttpGet("dynamicValidationError")]
        public IActionResult DynamicValidationError()
        {
            var errors = new List<ValidationFailure>
            {
                new ValidationFailure("age", "Age is not valid."),
                new ValidationFailure("id", Guid.NewGuid().ToString()),
                new ValidationFailure("name", "No person name was provided.")
            };
            
            throw new ValidationException(errors);
        }
    }
}
