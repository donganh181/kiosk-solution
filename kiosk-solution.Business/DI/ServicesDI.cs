using kiosk_solution.Business.Services;
using kiosk_solution.Business.Services.impl;
using kiosk_solution.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using kiosk_solution.Data.Repositories.impl;
using kiosk_solution.Business.Utilities;
using kiosk_solution.Data.Context;

namespace kiosk_solution.Business.DI
{
    public static class ServicesDI
    {
        public static void ConfigServicesDI(this IServiceCollection services)
        {

            services.AddScoped<DbContext, Kiosk_PlatformContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFileService, FirebaseStorageService>();
            services.AddScoped<IMapService, GoongMapService>();
            services.AddScoped<INotiService, FCMService>();
            services.AddScoped<IHomeService, HomeService>();

            services.AddScoped<IPartyRepository, PartyRepository>();
            services.AddScoped<IPartyService, PartyService>();
            
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IKioskRepository, KioskRepository>();
            services.AddScoped<IKioskService, KioskService>();

            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IScheduleService, ScheduleService>();
            
            services.AddScoped<IKioskLocationRepository, KioskLocationRepository>();
            services.AddScoped<IKioskLocationService, KioskLocationService>();

            services.AddScoped<ITemplateRepository, TemplateRepsitory>();
            services.AddScoped<ITemplateService, TemplateService>();

            services.AddScoped<IKioskScheduleTemplateRepository, KioskScheduleTemplateRepository>();
            services.AddScoped<IKioskScheduleTemplateService, KioskScheduleTemplateService>();

            services.AddScoped<IServiceApplicationRepository, ServiceApplicationRepository>();
            services.AddScoped<IServiceApplicationService, ServiceApplicationService>();

            services.AddScoped<IServiceApplicationPublishRequestRepository, ServiceApplicationPublishRequestRepository>();
            services.AddScoped<IServiceApplicationPublishRequestService, ServiceApplicationPublishRequestService>();

            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventService, EventService>();

            services.AddScoped<IPartyServiceApplicationRepository, PartyServiceApplicationRepository>();
            services.AddScoped<IPartyServiceApplicationService, PartyServiceApplicationService>();

            services.AddScoped<IProvinceService, ProvinceService>();

            services.AddScoped<IPoiRepository, PoiRepository>();
            services.AddScoped<IPoiService, PoiService>();

            services.AddScoped<IAppCategoryRepository, AppCategoryRepository>();
            services.AddScoped<IAppCategoryService, AppCategoryService>();

            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IImageService, ImageService>();

            services.AddScoped<IAppCategoryPositionRepository, AppCategoryPositionRepository>();
            services.AddScoped<IAppCategoryPositionService, AppCategoryPositionService>();

            services.AddScoped<IEventPositionRepository, EventPositionRepository>();
            services.AddScoped<IEventPositionService, EventPositionService>();

            services.AddScoped<IPoicategoryRepository, PoicategoryRepository>();
            services.AddScoped<IPoicategoryService, PoicategoryService>();

            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<IPartyNotificationRepository, PartyNotificationRepository>();
            services.AddScoped<IPartyNotificationService, PartyNotificationService>();

            services.AddScoped<IServiceApplicationFeedBackRepository, ServiceApplicationFeedBackRepository>();
            services.AddScoped<IServiceApplicationFeedBackService, ServiceApplicationFeedBackService>();

            services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();
            services.AddScoped<IServiceOrderService, ServiceOrderService>();

            services.AddScoped<IKioskRatingRepository, KioskRatingRepository>();
            services.AddScoped<IKioskRatingService, KioskRatingService>();
        }
    }
}
