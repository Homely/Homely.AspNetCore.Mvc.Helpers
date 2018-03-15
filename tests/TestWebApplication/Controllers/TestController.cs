using Microsoft.AspNetCore.Mvc;
using System;
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
        [HttpGet("{id}", Name ="GetId")]
        public IActionResult Get(int id)
        {
            var model = _fakeVehicleRepository.Get(id);

            return model == null 
                ? (IActionResult)NotFound() 
                : Ok(model);
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
    }
}
