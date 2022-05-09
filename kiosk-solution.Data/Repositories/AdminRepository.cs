using kiosk_solution.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace kiosk_solution.Data.Repositories
{
    public interface IAdminRepository : IBaseRepository<TblAdmin>
    {
    }
    public class AdminRepository : BaseRepository<TblAdmin>, IAdminRepository
    {
        public AdminRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
