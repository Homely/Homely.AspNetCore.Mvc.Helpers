using System;
using System.Collections.Generic;
using System.Linq;
using TestWebApplication.Models;

namespace TestWebApplication.Repositories
{
    public class FakeVehicleRepository : IFakeVehicleRepository
    {
        private readonly IDictionary<int, FakeVehicle> _fakeVehicles = new Dictionary<int, FakeVehicle>();

        public void Add(FakeVehicle fakeVehicle)
        {
            if (fakeVehicle == null)
            {
                throw new ArgumentNullException(nameof(fakeVehicle));
            }

            // NOTE: Obviously, not thread safe.
            fakeVehicle.Id = _fakeVehicles.Count + 1;

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

            _fakeVehicles.TryGetValue(id, out FakeVehicle fakeVehicle);
            return fakeVehicle;
        }

        public IQueryable<FakeVehicle> Index()
        {
            return _fakeVehicles.Values.AsQueryable();
        }
    }
}
