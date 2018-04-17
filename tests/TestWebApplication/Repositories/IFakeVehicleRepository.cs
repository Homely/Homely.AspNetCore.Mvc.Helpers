using System.Linq;
using TestWebApplication.Models;

namespace TestWebApplication.Repositories
{
    public interface IFakeVehicleRepository
    {
        IQueryable<FakeVehicle> Index();
        FakeVehicle Get(int id);
        void Add(FakeVehicle fakeVehicle);
        void Delete(int id);
    }
}
