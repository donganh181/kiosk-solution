using kiosk_solution.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace kiosk_solution.Data.Repositories
{
    public interface ILocationOwnerRepository : IBaseRepository<TblLocationOwner>
    {
    }
    public class LocationOwnerRepository : BaseRepository<TblLocationOwner>, ILocationOwnerRepository
    {
        public LocationOwnerRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
