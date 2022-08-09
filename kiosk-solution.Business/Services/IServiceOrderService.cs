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

        public Task<List<ServiceOrderCommissionSearchViewModel>> GetAllCommission(Guid partyId, Guid kioskId, ServiceOrderCommissionSearchViewModel model);
        public Task<ServiceOrderCommissionPieChartViewModel> GetAllCommissionParty(Guid partyId, Guid serviceApplicationId);
        public Task<ServiceOrderCommissionPieChartViewModel> GetAllCommissionPartyByMonth(Guid partyId, Guid serviceApplicationId , int month, int year);
        public Task<ServiceOrderCommissionPieChartViewModel> GetAllCommissionPartyByYear(Guid partyId, Guid serviceApplicationId , int year);
        public Task<ServiceOrderCommissionPieChartViewModel> GetAllCommissionKiosk(Guid partyId, Guid kioskId);
        public Task<ServiceOrderCommissionPieChartViewModel> GetAllCommissionKioskByMonth(Guid partyId, Guid kioskId, int month, int year);
        public Task<ServiceOrderCommissionPieChartViewModel> GetAllCommissionKioskByYear(Guid partyId, Guid kioskId, int year);
        public Task<ServiceOrderCommissionLineChartViewModel> GetAllCommissionKioskByMonthOfYear(Guid partyId, Guid kioskId, int year, List<Guid> serviceApplicationIds);
        public Task<ServiceOrderCommissionLineChartViewModel> GetAllCommissionKioskByDayOfMonth(Guid partyId, Guid kioskId,int month, int year, List<Guid> serviceApplicationIds);
    }
}