using kiosk_solution.Data.Models;
using System.Linq;

namespace kiosk_solution.Data.Repositories
{
    public interface IKioskRepository : IBaseRepository<Kiosk>
    {
        IQueryable<Kiosk> GetKioskNearBy(double longitude, double latitude);
    }
}