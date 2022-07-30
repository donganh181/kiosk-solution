using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services
{
    public interface IKioskRatingService
    {
        Task<KioskRatingViewModel> Create(KioskRatingCreateViewModel model);
        Task<KioskRatingViewModel> GetById(Guid id);
        Task<DynamicModelResponse<KioskRatingViewModel>> GetAllWithPagingByKioskId(Guid kioskId, int size, int pageNum);
        Task<Dictionary<int, double?>> GetAverageRatingOfKiosk(Guid kioskId);
        Task<List<KioskRatingViewModel>> GetListFeedbackByKioskId(Guid kioskId);
    }
}
