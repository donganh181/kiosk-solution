using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Services
{
    public interface IScheduleService
    {
        Task<ScheduleViewModel> CreateSchedule(Guid partyId, CreateScheduleViewModel model);
        Task<DynamicModelResponse<ScheduleViewModel>> GetAllWithPaging(Guid partyId, int size, int pageNum);
        Task<bool> IsOwner(Guid partyId, Guid scheduleId);
    }
}