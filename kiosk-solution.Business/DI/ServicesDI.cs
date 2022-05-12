using kiosk_solution.Business.Services;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace kiosk_solution.Business.DI
{
    public static class ServicesDI
    {
        public static void ConfigServicesDI(this IServiceCollection services)
        {

            services.AddScoped<DbContext, Kiosk_PlatformContext>();

            services.AddScoped<IPartyRepository, PartyRepository>();
            services.AddScoped<IPartyService, PartyService>();
        }
    }
}
