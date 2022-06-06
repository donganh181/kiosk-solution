using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services
{
    public interface IAppCategoryService
    {
        public Task<AppCategoryViewModel> Create(AppCategoryCreateViewModel model);
        public Task<AppCategoryViewModel> Update(AppCategoryUpdateViewModel model);
        public Task<DynamicModelResponse<AppCategorySearchViewModel>> GetAllWithPaging(AppCategorySearchViewModel model, int size, int pageNum);
    }
}
