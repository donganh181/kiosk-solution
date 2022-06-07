using kiosk_solution.Data.Context;
using kiosk_solution.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.Repositories.impl
{
    public class ImageRepository : BaseRepository<Image>, IImageRepository
    {
        public ImageRepository(Kiosk_PlatformContext dbContext) : base(dbContext)
        {
        }
    }
}
