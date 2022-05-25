using System;
using System.Threading.Tasks;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Services
{
    public interface IKioskService
    {
        Task<KioskViewModel> UpdateStatus(Guid updaterId, Guid kioskId);
        Task<KioskViewModel> CreateNewKiosk(CreateKioskViewModel model);
        Task<KioskViewModel> UpdateInformation(Guid updaterId, UpdateKioskViewModel model);
        Task<DynamicModelResponse<KioskSearchViewModel>> GetAllWithPaging(string role, Guid id, KioskSearchViewModel model, int size, int pageNum);
    }
}