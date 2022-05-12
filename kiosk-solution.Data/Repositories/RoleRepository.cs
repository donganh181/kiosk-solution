using kiosk_solution.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.Repositories
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
    }
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
