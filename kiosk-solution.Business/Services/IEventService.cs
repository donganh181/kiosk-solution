using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kiosk_solution.Data.Responses;

namespace kiosk_solution.Business.Services
{
    public interface IEventService
    {
        public Task<EventViewModel> Create(Guid creatorId, string role, EventCreateViewModel model);
        public Task<DynamicModelResponse<EventSearchViewModel>> GetAllWithPaging(Guid partyId, string roleName, EventSearchViewModel model, int size, int pageNum);
        public Task<EventViewModel> Update(Guid partyId, EventUpdateViewModel model, string roleName);
        public Task<EventImageViewModel> AddImageToEvent(Guid partyId, string roleName, EventAddImageViewModel model);
        public Task<ImageViewModel> UpdateImageToEvent(Guid partyId, string roleName, EventUpdateImageViewModel model);
    }
}
