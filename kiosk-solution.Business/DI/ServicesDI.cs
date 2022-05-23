using kiosk_solution.Business.Services;
using kiosk_solution.Business.Services.impl;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using kiosk_solution.Data.Repositories.impl;

namespace kiosk_solution.Business.DI
{
    public static class ServicesDI
    {
        public static void ConfigServicesDI(this IServiceCollection services)
        {

            services.AddScoped<DbContext, Kiosk_PlatformContext>();
            services.AddScoped<IPartyRepository, PartyRepository>();
            services.AddScoped<IPartyService, PartyService>();
            
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IKioskRepository, KioskRepository>();
            services.AddScoped<IKioskService, KioskService>();

            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IScheduleService, ScheduleService>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
