using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services
{
    public interface IEventService
    {
        public Task<EventViewModel> Create(Guid creatorId, string role, EventCreateViewModel model);
    }
}
