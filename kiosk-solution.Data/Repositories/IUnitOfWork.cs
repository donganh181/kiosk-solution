using System.Threading.Tasks;
using kiosk_solution.Data.Models;

namespace kiosk_solution.Data.Repositories
{
    public interface IUnitOfWork
    {
        IPartyRepository PartyRepository { get; }
        IRoleRepository RoleRepository { get; }
        IKioskRepository KioskRepository { get; }
        IScheduleRepository ScheduleRepository { get; }
        IKioskLocationRepository KioskLocationRepository { get; }
        ITemplateRepository TemplateRepository { get; }
        IKioskScheduleTemplateRepository KioskScheduleTemplateRepository { get; }
        IServiceApplicationRepository ServiceApplicationRepository { get; }
        IServiceApplicationPublishRequestRepository ServiceApplicationPublishRequestRepository { get; }
        IEventRepository EventRepository { get; }
        IPartyServiceApplicationRepository PartyServiceApplicationRepository { get; }
        IPoiRepository PoiRepository { get; }
        IAppCategoryRepository AppCategoryRepository { get; }
        IImageRepository ImageRepository { get; }
        IAppCategoryPositionRepository AppCategoryPositionRepository { get; }
        IEventPositionRepository EventPositionRepository { get; }
        IPoicategoryRepository PoicategoryRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IPartyNotificationRepository PartyNotificationRepository { get; }
        IServiceApplicationFeedBackRepository ServiceApplicationFeedBackRepository { get; }
        IServiceOrderRepository ServiceOrderRepository { get; }
        IKioskRatingRepository KioskRatingRepository { get; }
        void Save();
        Task SaveAsync();
    }
}
