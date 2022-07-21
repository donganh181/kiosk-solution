using kiosk_solution.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.Repositories
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        public IQueryable<Event> GetEventNearBy(Guid partyId, double longitude, double latitude);
    }
}
