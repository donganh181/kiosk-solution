using System;
using System.Threading.Tasks;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Services
{
    public interface IPoicategoryService
    {
        public Task<PoicategoryViewModel> Create(PoiCategoryCreateViewModel model);
        public Task<PoicategoryViewModel> Update(PoiCategoryUpdateViewModel model);
        public Task<PoicategoryViewModel> Delete(Guid poiCategoryId);
        public Task<DynamicModelResponse<PoiCategorySearchViewModel>> GetAllWithPaging(PoiCategorySearchViewModel model, int size, int pageNum);
    }
}