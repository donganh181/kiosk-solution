using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Context;
using kiosk_solution.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.Repositories.impl
{
    public class EventRepository : BaseRepository<Event> , IEventRepository
    {
        public EventRepository(Kiosk_PlatformContext dbContext) : base(dbContext)
        {
        }
        public IQueryable<Event> GetEventNearBy(Guid partyId, double longitude, double latitude)
        {
            var result = dbContext.Events.Where(x =>
                            (Math.Sqrt(Math.Pow(69.1 * (latitude - (double)x.Latitude), 2) +
                            Math.Pow(69.1 * (double)(x.Longtitude - longitude) * Math.Cos(latitude / 57.3), 2))) * 1.609344 < 5
                            && (x.Type.Equals(TypeConstants.SERVER_TYPE) || (x.Type.Equals(TypeConstants.LOCAL_TYPE) && x.CreatorId.Equals(partyId)))
                            && (x.Status.Equals(StatusConstants.ON_GOING) || x.Status.Equals(StatusConstants.COMING_SOON))
                            && !x.Status.Equals(StatusConstants.DELETED) && !x.Status.Equals(StatusConstants.END))

                        .OrderBy(x =>
                            (Math.Sqrt(Math.Pow(69.1 * (latitude - (double)x.Latitude), 2) +
                            Math.Pow(69.1 * (double)(x.Longtitude - longitude) * Math.Cos(latitude / 57.3), 2))) * 1.609344);

            return result;
        }

    }
}
