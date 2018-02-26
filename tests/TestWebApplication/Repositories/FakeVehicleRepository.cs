using System;
using System.Collections.Generic;
using System.Linq;
using TestWebApplication.Models;

namespace TestWebApplication.Repositories
{
    public class FakeVehicleRepository : IFakeVehicleRepository
    {
        private IDictionary<int, FakeVehicle> _fakeVehicles = new Dictionary<int, FakeVehicle>();

        public void Add(FakeVehicle fakeVehicle)
        {
            if (fakeVehicle == null)
            {
                throw new ArgumentNullException(nameof(fakeVehicle));
            }

            fakeVehicle.Id = _fakeVehicles.Count;

            _fakeVehicles.Add(fakeVehicle.Id, fakeVehicle);
        }

        public void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            _fakeVehicles.Remove(id);
        }

        public FakeVehicle Get(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return _fakeVehicles[id];
        }

        public IQueryable<FakeVehicle> Index()
        {
            return _fakeVehicles.Values.AsQueryable();
        }
    }
}
