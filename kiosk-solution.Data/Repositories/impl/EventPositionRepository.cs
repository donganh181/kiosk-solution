using kiosk_solution.Data.Context;
using kiosk_solution.Data.Models;

namespace kiosk_solution.Data.Repositories.impl
{
    public class EventPositionRepository : BaseRepository<EventPosition>, IEventPositionRepository
    {
        public EventPositionRepository(Kiosk_PlatformContext dbContext) : base(dbContext)
        {
        }
    }
}