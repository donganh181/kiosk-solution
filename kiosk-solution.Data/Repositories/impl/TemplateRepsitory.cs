using kiosk_solution.Data.Context;
using kiosk_solution.Data.Models;

namespace kiosk_solution.Data.Repositories.impl
{
    public class TemplateRepsitory : BaseRepository<Template>, ITemplateRepository
    {
        public TemplateRepsitory(Kiosk_PlatformContext dbContext) : base(dbContext)
        {
        }
    }
}