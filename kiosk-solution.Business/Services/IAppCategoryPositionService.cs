using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kiosk_solution.Data.Responses;

namespace kiosk_solution.Business.Services
{
    public interface IAppCategoryPositionService
    {
        public Task<AppCategoryPositionViewModel> Create(Guid partyId, AppCategoryPositionCreateViewModel model);
        public Task<AppCategoryPositionViewModel> Update(Guid partyId, AppCategoryPositionUpdateViewModel model);
        public Task<AppCategoryPositionGetViewModel> GetById(Guid partyId, Guid templateId);
    }
}
