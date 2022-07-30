using System;
using System.Threading.Tasks;
using kiosk_solution.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace kiosk_solution.Data.Repositories.impl
{
    public class UnitOfWork : IUnitOfWork
    {
        public Kiosk_PlatformContext _context { get; set; }
        public IRoleRepository RoleRepository { get; set; }
        public IKioskRepository KioskRepository { get; set; }
        public IPartyRepository PartyRepository { get; set; }
        public IKioskLocationRepository KioskLocationRepository { get; set; }
        public IScheduleRepository ScheduleRepository { get; set; }
        public ITemplateRepository TemplateRepository { get; set; }
        public IKioskScheduleTemplateRepository KioskScheduleTemplateRepository { get; set; }
        public IServiceApplicationRepository ServiceApplicationRepository { get; set; }
        public IServiceApplicationPublishRequestRepository ServiceApplicationPublishRequestRepository { get; set; }
        public IEventRepository EventRepository { get; set; }
        public IPartyServiceApplicationRepository PartyServiceApplicationRepository { get; set; }
        public IPoiRepository PoiRepository { get; set; }
        public IAppCategoryRepository AppCategoryRepository { get; set; }
        public IImageRepository ImageRepository { get; set; }
        public IAppCategoryPositionRepository AppCategoryPositionRepository { get; set; }
        public IEventPositionRepository EventPositionRepository { get; set; }
        public IPoicategoryRepository PoicategoryRepository { get; set; }
        public INotificationRepository NotificationRepository { get; set; }
        public IPartyNotificationRepository PartyNotificationRepository { get; set; }
        public IServiceApplicationFeedBackRepository ServiceApplicationFeedBackRepository { get; set; }
        public IServiceOrderRepository ServiceOrderRepository { get; }

        public IKioskRatingRepository KioskRatingRepository { get; }

        public UnitOfWork(Kiosk_PlatformContext context, IRoleRepository roleRepository,
            IKioskRepository kioskRepository,
            IPartyRepository partyRepository, IKioskLocationRepository kioskLocationRepository,
            IScheduleRepository scheduleRepository,
            ITemplateRepository templateRepository, IKioskScheduleTemplateRepository kioskScheduleTemplateRepository,
            IServiceApplicationRepository serviceApplicationRepository,
            IServiceApplicationPublishRequestRepository serviceApplicationPublishRequestRepository,
            IEventRepository eventRepository,
            IPartyServiceApplicationRepository partyServiceApplicationRepository, IPoiRepository poiRepository,
            IAppCategoryRepository appCategoryRepository, IImageRepository imageRepository,
            IAppCategoryPositionRepository appCategoryPositionRepository,
            IEventPositionRepository eventPositionRepository, IPoicategoryRepository poicategoryRepository,
            INotificationRepository notificationRepository,
            IPartyNotificationRepository partyNotificationRepository,
            IServiceApplicationFeedBackRepository serviceApplicationFeedBackRepository,
            IServiceOrderRepository serviceOrderRepository,
            IKioskRatingRepository kioskRatingRepository)
        {
            _context = context;
            RoleRepository = roleRepository;
            KioskRepository = kioskRepository;
            PartyRepository = partyRepository;
            KioskLocationRepository = kioskLocationRepository;
            ScheduleRepository = scheduleRepository;
            TemplateRepository = templateRepository;
            KioskScheduleTemplateRepository = kioskScheduleTemplateRepository;
            ServiceApplicationRepository = serviceApplicationRepository;
            ServiceApplicationPublishRequestRepository = serviceApplicationPublishRequestRepository;
            EventRepository = eventRepository;
            PartyServiceApplicationRepository = partyServiceApplicationRepository;
            PoiRepository = poiRepository;
            AppCategoryRepository = appCategoryRepository;
            ImageRepository = imageRepository;
            AppCategoryPositionRepository = appCategoryPositionRepository;
            EventPositionRepository = eventPositionRepository;
            PoicategoryRepository = poicategoryRepository;
            NotificationRepository = notificationRepository;
            PartyNotificationRepository = partyNotificationRepository;
            ServiceApplicationFeedBackRepository = serviceApplicationFeedBackRepository;
            ServiceOrderRepository = serviceOrderRepository;
            KioskRatingRepository = kioskRatingRepository;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}