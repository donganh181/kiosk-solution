using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Services
{
    public interface IScheduleService
    {
        Task<ScheduleViewModel> Create(Guid partyId, CreateScheduleViewModel model);
        Task<ScheduleViewModel> Update(Guid partyId, ScheduleUpdateViewModel model);
        Task<DynamicModelResponse<ScheduleViewModel>> GetAllWithPaging(Guid partyId, int size, int pageNum);
        Task<bool> IsOwner(Guid partyId, Guid scheduleId);
        Task<Schedule> GetById(Guid scheduleId);
        Task<ScheduleViewModel> ClientGetById(Guid partyId, Guid scheduleId);
        Task<ScheduleViewModel> ChangeStatus(Guid partyId, Guid scheduleId);
    }
}