using System;
using System.Threading.Tasks;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Services
{
    public interface IEventPositionService
    {
        public Task<EventPositionViewModel> Create(Guid partyId, EventPositionCreateViewModel model);
        public Task<EventPositionViewModel> Update(Guid partyId, EventPositionUpdateViewModel model);
        public Task<EventPositionGetViewModel> GetById(Guid partyId, Guid templateId);
    }
}