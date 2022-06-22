using kiosk_solution.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kiosk_solution.Data.Repositories
{
    public interface IPoiRepository : IBaseRepository<Poi>
    {
        public IQueryable<Poi> GetPoiNearBy(Guid partyId, double longitude, double latitude);
    }
}