using AutoMapper;
using kiosk_solution.Data.AutoMapper;
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
                mc.ConfigPartyModule();
                mc.ConfigRoleModule();
                mc.ConfigKioskModule();
                mc.ConfigKioskLocationModule();
                mc.ConfigScheduleModule();
                mc.ConfigKioskScheduleTemplateModule();
                mc.ConfigServiceApplicationModule();
                mc.ConfigServiceApplicationPublishRequestModule();
                mc.ConfigTemplateModule();
                mc.ConfigEventModule();
                mc.ConfigPartyServiceApplicationModule();
                mc.ConfigPoiModule();
                mc.ConfigAppCategoryModule();
                mc.ConfigImageModule();
                mc.ConfigAppCategoryPositionModule();
                mc.ConfigEventPositionModule();
                mc.ConfigPoicategoryModule();
                mc.ConfigNotificationModule();
                mc.ConfigPartyNotificationModule();
                mc.ConfigServiceApplicationFeedBackModule();
                mc.ConfigServiceOrderModule();
                mc.ConfigKioskRatingModule();
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
