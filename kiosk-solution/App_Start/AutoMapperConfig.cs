using AutoMapper;

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kiosk_solution.App_Start
{
    public static class AutoMapperConfig
    {
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
