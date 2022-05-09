using kiosk_solution.Business.DI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kiosk_solution.App_Start
{
    public static class DependencyInjectionResolver
    {
        public static void ConfigureDI(this IServiceCollection services)
        {
            services.ConfigServicesDI();
            services.ConfigureAutoMapper();
        }
    }
}
