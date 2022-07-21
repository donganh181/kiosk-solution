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
        Task<EventViewModel> Create(Guid creatorId, string role, EventCreateViewModel model);
        Task<DynamicModelResponse<EventSearchViewModel>> GetAllWithPaging(Guid? partyId, string roleName, EventSearchViewModel model, int size, int pageNum);
        Task<EventViewModel> Update(Guid partyId, EventUpdateViewModel model, string roleName);
        Task<EventImageViewModel> AddImageToEvent(Guid partyId, string roleName, EventAddImageViewModel model);
        Task<ImageViewModel> UpdateImageToEvent(Guid partyId, string roleName, EventUpdateImageViewModel model);
        Task<EventViewModel> DeleteImageFromEvent(Guid partyId, string roleName, Guid imageId);
        Task<EventViewModel> Delete(Guid partyId, string roleName, Guid eventId);
        Task<EventViewModel> GetById(Guid id);
        Task<EventViewModel> ReplaceImage(Guid partyId, string roleName, ImageReplaceViewModel model);
        public Task<DynamicModelResponse<EventNearbySearchViewModel>> GetEventNearby(Guid partyId, EventNearbySearchViewModel model, int size, int pageNum);
    }
}
