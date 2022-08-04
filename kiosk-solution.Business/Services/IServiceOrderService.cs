using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Services
{
    public interface IServiceOrderService
    {
        public Task<ServiceOrderViewModel> Create(ServiceOrderCreateViewModel model);

        public Task<DynamicModelResponse<ServiceOrderSearchViewModel>> GetAllWithPaging(Guid partyId,
            ServiceOrderSearchViewModel model, int size, int pageNum);

        public Task<List<ServiceOrderCommissionSearchViewModel>> GetAllCommission(Guid partyId, Guid kioskId);
        public Task<List<ServiceOrderCommissionSearchViewModel>> GetAllCommissionByMonth(Guid partyId, Guid kioskId, int month, int year);
    }
}