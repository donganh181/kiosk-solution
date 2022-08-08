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
        Task<KioskLocationViewModel> CreateNew(Guid partyId, CreateKioskLocationViewModel model);
        Task<KioskLocationViewModel> UpdateInformation(Guid partyId, UpdateKioskLocationViewModel model);
        Task<DynamicModelResponse<KioskLocationSearchViewModel>> GetAllWithPaging(Guid partyId, KioskLocationSearchViewModel model, int size, int pageNum);
        Task<KioskLocationViewModel> ReplaceImage(Guid partyId, ImageReplaceViewModel model);
        Task<KioskLocationViewModel> GetById(Guid id, bool isNotDes);
        Task<KioskLocationViewModel> GetByIdAndChangeKioskView(Guid id);
    }
}
