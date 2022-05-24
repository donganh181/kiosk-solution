using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services
{
    public interface IKioskLocationService
    {
        Task<KioskLocationViewModel> CreateNew(CreateKioskLocationViewModel model);
        Task<DynamicModelResponse<KioskLocationSearchViewModel>> GetAllWithPaging(KioskLocationSearchViewModel model, int size, int pageNum);
    }
}
