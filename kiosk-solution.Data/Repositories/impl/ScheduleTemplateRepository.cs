using kiosk_solution.Data.Models;

namespace kiosk_solution.Data.Repositories.impl
{
    public class ScheduleTemplateRepository : BaseRepository<ScheduleTemplate>, IScheduleTemplateRepository
    {
        public ScheduleTemplateRepository(Kiosk_PlatformContext dbContext) : base(dbContext)
        {
        }
    }
}