using System;
using System.Threading.Tasks;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Services
{
    public interface IPoiService
    {
        public Task<PoiViewModel> Create(Guid partyId, string roleName, PoiCreateViewModel model);
        public Task<DynamicModelResponse<PoiSearchViewModel>> GetWithPaging(Guid partyId, string roleName, PoiSearchViewModel model, int size, int pageNum);
    }
}