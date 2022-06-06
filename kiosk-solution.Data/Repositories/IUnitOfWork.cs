using System.Threading.Tasks;
using kiosk_solution.Data.Models;

namespace kiosk_solution.Data.Repositories
{
    public interface IUnitOfWork
    {
        IPartyRepository  PartyRepository { get; }
        IRoleRepository RoleRepository { get; }
        IKioskRepository KioskRepository { get; }
        IScheduleRepository ScheduleRepository { get; }
        IKioskLocationRepository KioskLocationRepository { get; }
        ITemplateRepository TemplateRepository { get; }
        IScheduleTemplateRepository ScheduleTemplateRepository { get; }
        IServiceApplicationRepository ServiceApplicationRepository { get; }
        IServiceApplicationPublishRequestRepository ServiceApplicationPublishRequestRepository { get; }
        IEventRepository EventRepository { get; }
        IPartyServiceApplicationRepository PartyServiceApplicationRepository { get; }
        IPoiRepository PoiRepository { get; }
        void Save();
        Task SaveAsync();
    }
}
