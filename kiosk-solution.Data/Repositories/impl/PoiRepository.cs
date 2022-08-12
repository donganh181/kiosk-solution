using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Context;
using kiosk_solution.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kiosk_solution.Data.Repositories.impl
{
    public class PoiRepository : BaseRepository<Poi>, IPoiRepository
    {
        public PoiRepository(Kiosk_PlatformContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<Poi> GetPoiNearBy(Guid partyId, double longitude, double latitude)
        {
            var result = dbContext.Pois.Where(x =>
                            (Math.Sqrt(Math.Pow(69.1 * (latitude - (double)x.Latitude), 2) +
                            Math.Pow(69.1 * (double)(x.Longtitude - longitude) * Math.Cos(latitude / 57.3), 2))) * 1.609344 < 5
                            && x.Status.Equals(StatusConstants.ACTIVATE)
                            && (x.Type.Equals(TypeConstants.SERVER_TYPE) || (x.Type.Equals(TypeConstants.LOCAL_TYPE) && x.CreatorId.Equals(partyId))))
              
                        .OrderBy(x => 
                            (Math.Sqrt(Math.Pow(69.1 * (latitude - (double)x.Latitude), 2) +
                            Math.Pow(69.1 * (double)(x.Longtitude - longitude) * Math.Cos(latitude / 57.3), 2))) * 1.609344);

            return result;
        }
    }
}