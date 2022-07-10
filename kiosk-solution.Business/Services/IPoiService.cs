using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Services
{
    public interface IPoiService
    {
        public Task<PoiViewModel> Create(Guid partyId, string roleName, PoiCreateViewModel model);
        public Task<PoiViewModel> UpdateInformation(Guid partyId, string roleName, PoiInfomationUpdateViewModel model);
        public Task<DynamicModelResponse<PoiSearchViewModel>> GetAllWithPaging(Guid partyId, string role, PoiSearchViewModel model, int size, int pageNum);
        public Task<PoiSearchViewModel> GetById(Guid id);
        public Task<PoiImageViewModel> AddImageToPoi(Guid partyId, string roleName, PoiAddImageViewModel model);
        public Task<ImageViewModel> UpdateImageToPoi(Guid partyId, string roleName, PoiUpdateImageViewModel model);
        public Task<PoiViewModel> DeleteImageFromPoi(Guid partyId, string roleName, Guid imageId);
        public Task<DynamicModelResponse<PoiNearbySearchViewModel>> GetLocationNearby(Guid kioskId, PoiNearbySearchViewModel model, int size, int pageNum);
        public Task<PoiViewModel> ReplaceImage(Guid partyId, string roleName, ImageReplaceViewModel model);
    }
}
