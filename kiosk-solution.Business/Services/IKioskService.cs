using System;
using System.Threading.Tasks;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Services
{
    public interface IKioskService
    {
        Task<KioskViewModel> UpdateStatus(Guid updaterId, Guid kioskId);
        Task<KioskViewModel> CreateNewKiosk(CreateKioskViewModel model);
    }
}