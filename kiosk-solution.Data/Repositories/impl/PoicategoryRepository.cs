using kiosk_solution.Data.Context;
using kiosk_solution.Data.Models;

namespace kiosk_solution.Data.Repositories.impl
{
    public class PoicategoryRepository : BaseRepository<Poicategory>, IPoicategoryRepository
    {
        public PoicategoryRepository(Kiosk_PlatformContext dbContext) : base(dbContext)
        {
        }
    }
}