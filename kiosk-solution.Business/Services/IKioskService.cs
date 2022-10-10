using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Services
{
    public interface IKioskService
    {
        Task<KioskViewModel> UpdateStatus(Guid updaterId, Guid kioskId, bool isKioskSetup);
        Task<KioskViewModel> CreateNewKiosk(CreateKioskViewModel model);
        Task<KioskViewModel> UpdateInformation(Guid updaterId, UpdateKioskViewModel model);
        Task<KioskViewModel> UpdateKioskName(Guid updaterId, KioskNameUpdateViewModel model);

        Task<DynamicModelResponse<KioskSearchViewModel>> GetAllWithPaging(string role, Guid id,
            KioskSearchViewModel model, int size, int pageNum);

        Task<KioskViewModel> GetById(Guid kioskId);
        Task<KioskViewModel> GetByIdWithParyId(Guid kioskId, Guid partyId);
        Task<KioskViewModel> AddDeviceId(KioskAddDeviceIdViewModel model);
        Task<List<KioskDetailViewModel>> GetListSpecificKiosk();
        Task<dynamic> GetSpecificKiosk(Guid id);

        Task<DynamicModelResponse<KioskNearbyViewModel>> GetKioskNearby(KioskNearbyViewModel model, int size,
            int pageNum);

        Task<List<Kiosk>> GetListKioskByKioskLocationId(Guid id);
        Task<CountViewModel> CountKiosks(Guid partyId, string role);
        Task<string> GetNameById(Guid kioskId);
    }
}