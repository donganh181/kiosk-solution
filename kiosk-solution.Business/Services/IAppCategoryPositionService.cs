using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services
{
    public interface IAppCategoryPositionService
    {
        public Task<AppCategoryPositionViewModel> Create(Guid id, AppCategoryPositionCreateViewModel model);
    }
}
