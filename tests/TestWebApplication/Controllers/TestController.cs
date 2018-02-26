using Microsoft.AspNetCore.Mvc;
using System;
using TestWebApplication.Models;
using TestWebApplication.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestWebApplication.Controllers
{
    public class TestController : Controller
    {
        private readonly IFakeVehicleRepository _fakeVehicleRepository;

        public TestController(IFakeVehicleRepository fakeVehicleRepository)
        {
            _fakeVehicleRepository = fakeVehicleRepository ?? throw new ArgumentNullException(nameof(fakeVehicleRepository));
        }

        // GET: /test/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var model = _fakeVehicleRepository.Get(id);

            return model == null 
                ? (IActionResult)NotFound() 
                : Ok(model);
        }

        // POST: /test/Create
        [HttpPost]
        public IActionResult Create(FakeVehicle fakeVehicle)
        {
            _fakeVehicleRepository.Add(fakeVehicle);

            return CreatedAtAction("Get", new { id = fakeVehicle.Id });
            //return Created("https://www.localhost.com/test/fakeVehicle/1", null);
        }

        // GET: /test/error
        [HttpGet]
        public IActionResult Error()
        {
            throw new System.Exception("Something bad ass happened.");
        }
    }
}
