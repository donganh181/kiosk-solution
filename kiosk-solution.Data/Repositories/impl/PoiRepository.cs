using kiosk_solution.Data.Context;
using kiosk_solution.Data.Models;

namespace kiosk_solution.Data.Repositories.impl
{
    public class PoiRepository : BaseRepository<Poi>, IPoiRepository
    {
        public PoiRepository(Kiosk_PlatformContext dbContext) : base(dbContext)
        {
        }
    }
}