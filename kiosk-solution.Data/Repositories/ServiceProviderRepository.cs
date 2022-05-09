using kiosk_solution.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace kiosk_solution.Data.Repositories
{
    public interface IServiceProviderRepository : IBaseRepository<TblServiceProvider>
    {
    }
    public class ServiceProviderRepository : BaseRepository<TblServiceProvider>, IServiceProviderRepository
    {
        public ServiceProviderRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
