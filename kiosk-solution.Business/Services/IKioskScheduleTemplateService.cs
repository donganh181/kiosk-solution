using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Services
{
    public interface IKioskScheduleTemplateService
    {
        Task<KioskScheduleTemplateViewModel> Create(Guid partyId, KioskScheduleTemplateCreateViewModel model);
        Task<KioskScheduleTemplateViewModel> Delete(Guid partyId, KioskScheduleTemplateDeleteViewModel model);
        Task<KioskScheduleTemplateViewModel> Update(Guid partyId, KioskScheduleTemplateUpdateViewModel model);
        Task<KioskScheduleTemplateViewModel> ChangeStatus(Guid partyId, Guid kioskScheduleTemplateId);
        Task<List<KioskScheduleTemplateViewModel>> ChangeStatusByTemplateId(Guid partyId, Guid templateId);
        Task<List<KioskScheduleTemplateViewModel>> ChangeStatusByScheduleId(Guid partyId, Guid scheduleId);
        Task<DynamicModelResponse<KioskScheduleTemplateViewModel>> GetByKioskId(Guid kioskId, Guid partyId, int size, int pageNum);
    }
}