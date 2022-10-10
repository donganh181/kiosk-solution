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

        public Task<List<ServiceOrderCommissionSearchViewModel>> GetAllCommission(Guid partyId, ServiceOrderCommissionSearchViewModel model);
        public Task<DynamicModelResponse<ServiceOrderLocationOwnerViewModel>> GetAllOrderByApplicationWithPaging(Guid partyId, Guid serviceApplicationId, int size, int pageNum);
        public Task<ServiceOrderCommissionPieChartViewModel> GetAllCommissionKiosk(Guid partyId, Guid kioskId);
        //app By Kiosk - location owner
        public Task<ServiceOrderCommissionPieChartViewModel> GetAllCommissionKioskByMonth(Guid partyId, Guid kioskId, int month, int year);
        public Task<ServiceOrderCommissionPieChartViewModel> GetAllCommissionKioskByYear(Guid partyId, Guid kioskId, int year);
        public Task<ServiceOrderCommissionLineChartViewModel> GetAllCommissionKioskByMonthOfYear(Guid partyId, Guid kioskId, int year, List<Guid> serviceApplicationIds);
        public Task<ServiceOrderCommissionLineChartViewModel> GetAllCommissionKioskByDayOfMonth(Guid partyId, Guid kioskId,int month, int year, List<Guid> serviceApplicationIds);
        //kiosk - location owner
        public Task<ServiceOrderCommissionLineChartByKioskViewModel> GetAllCommissionMonthOfYearByKiosk(Guid partyId, int year, List<Guid> kioskIds);
        public Task<ServiceOrderCommissionLineChartByKioskViewModel> GetAllCommissionDayOfMonthByKiosk(Guid partyId, int month, int year, List<Guid> kioskIds);
        //app by kiosk - admin
        public Task<ServiceOrderCommissionPieChartViewModel> GetAllCommissionSystemByMonth(int month, int year);
        public Task<ServiceOrderCommissionPieChartViewModel> GetAllCommissionSystemByYear(int year);
        public Task<ServiceOrderCommissionLineChartViewModel> GetAllCommissionSystemByMonthOfYear(int year, List<Guid> serviceApplicationIds);
        public Task<ServiceOrderCommissionLineChartViewModel> GetAllCommissionSystemByDayOfMonth(int month, int year, List<Guid> serviceApplicationIds);
    }
}