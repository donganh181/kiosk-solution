using System;
using System.Threading.Tasks;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Services
{
    public interface IKioskScheduleTemplateService
    {
        Task<KioskScheduleTemplateViewModel> AddTemplateToSchedule(Guid partyId, KioskScheduleTemplateCreateViewModel model);
        Task<DynamicModelResponse<KioskScheduleTemplateViewModel>> GetByKioskId(Guid kioskId, Guid partyId, int size, int pageNum);
    }
}