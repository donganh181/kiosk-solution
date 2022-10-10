using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Services
{
    public interface IPoiService
    {
        Task<PoiViewModel> UpdateInformation(Guid partyId, string roleName, PoiInfomationUpdateViewModel model);
        Task<DynamicModelResponse<PoiSearchViewModel>> GetAllWithPaging(Guid partyId, string role, PoiSearchViewModel model, int size, int pageNum);
        Task<PoiSearchViewModel> GetById(Guid id);
        Task<PoiImageViewModel> AddImageToPoi(Guid partyId, string roleName, PoiAddImageViewModel model);
        Task<ImageViewModel> UpdateImageToPoi(Guid partyId, string roleName, PoiUpdateImageViewModel model);
        Task<PoiViewModel> DeleteImageFromPoi(Guid partyId, string roleName, Guid imageId);
        Task<DynamicModelResponse<PoiNearbySearchViewModel>> GetLocationNearby(Guid kioskId, PoiNearbySearchViewModel model, int size, int pageNum);
        Task<PoiViewModel> ReplaceImage(Guid partyId, string roleName, ImageReplaceViewModel model);
        Task<bool> IsExistPoiInCategory(Guid poiCategoryId);
        Task<CountViewModel> CountPOIs(Guid partyId, string role);
        Task<PoiViewModel> UpdateStatus(Guid partyId, string roleName, Guid poiId);
        Task<List<PoiViewModel>> GetListPoiByPartyId(Guid partyId, double longitude, double latitude);
        Task<PoiViewModel> Create(Guid partyId, string roleName, PoiCreateViewModel model);
        Task<PoiViewModel> UpdateBanner(Guid partyId, PoiUpdateBannerViewModel model);
    }
}
