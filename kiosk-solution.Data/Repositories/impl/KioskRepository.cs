using kiosk_solution.Data.Context;
using kiosk_solution.Data.Models;

namespace kiosk_solution.Data.Repositories.impl
{
    public class KioskRepository : BaseRepository<Kiosk>, IKioskRepository
    {
        public KioskRepository(Kiosk_PlatformContext dbContext) : base(dbContext)
        {
        }
    }
}