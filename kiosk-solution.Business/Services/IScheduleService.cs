using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Services
{
    public interface IScheduleService
    {
        Task<ScheduleViewModel> CreateSchedule(Guid id, CreateScheduleViewModel model);
        Task<List<ScheduleViewModel>> GetAll(Guid id);
    }
}