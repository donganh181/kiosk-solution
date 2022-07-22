using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Context;
using kiosk_solution.Data.Models;
using System;
using System.Linq;

namespace kiosk_solution.Data.Repositories.impl
{
    public class KioskRepository : BaseRepository<Kiosk>, IKioskRepository
    {
        public KioskRepository(Kiosk_PlatformContext dbContext) : base(dbContext)
        {
        }
        public IQueryable<Kiosk> GetKioskNearBy(double longitude, double latitude)
        {
            var result = dbContext.Kiosks.Where(x =>
                            (Math.Sqrt(Math.Pow(69.1 * (latitude - (double)x.Latitude), 2) +
                            Math.Pow(69.1 * (double)(x.Longtitude - longitude) * Math.Cos(latitude / 57.3), 2))) * 1.609344 < 5
                            && x.Status.Equals(StatusConstants.ACTIVATE))

                        .OrderByDescending(x =>
                            (Math.Sqrt(Math.Pow(69.1 * (latitude - (double)x.Latitude), 2) +
                            Math.Pow(69.1 * (double)(x.Longtitude - longitude) * Math.Cos(latitude / 57.3), 2))) * 1.609344);

            return result;
        }
    }
}