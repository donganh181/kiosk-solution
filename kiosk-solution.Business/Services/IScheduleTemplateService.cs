using System;
using System.Threading.Tasks;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Services
{
    public interface IScheduleTemplateService
    {
        Task<ScheduleTemplateViewModel> AddTemplateToSchedule(Guid partyId, AddTemplateViewModel model);
    }
}