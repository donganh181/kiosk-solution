using kiosk_solution.Data.Context;
using kiosk_solution.Data.Models;

namespace kiosk_solution.Data.Repositories.impl
{
    public class KioskScheduleTemplateRepository : BaseRepository<KioskScheduleTemplate>, IKioskScheduleTemplateRepository
    {
        public KioskScheduleTemplateRepository(Kiosk_PlatformContext dbContext) : base(dbContext)
        {
        }
    }
}