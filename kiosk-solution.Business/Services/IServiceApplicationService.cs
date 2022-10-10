using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services
{
    public interface IServiceApplicationService
    {
        Task<ServiceApplicationViewModel> UpdateInformation(Guid updaterId, UpdateServiceApplicationViewModel model);
        Task<ServiceApplicationViewModel> UpdateBanner(Guid partyId, ServiceApplicationUpdateBannerViewModel model);
        Task<DynamicModelResponse<ServiceApplicationSearchViewModel>> GetAllWithPaging(string role, Guid? id, ServiceApplicationSearchViewModel model, int size, int pageNum);
        Task<ServiceApplicationViewModel> Create(Guid partyId, CreateServiceApplicationViewModel model);
        Task<ServiceApplicationSpecificViewModel> GetById(Guid? partyId, Guid id);
        Task<string> GetNameById(Guid serviceApplicationId);
        Task<ServiceApplicationCommissionViewModel> GetCommissionById(Guid serviceApplicationId);
        Task<bool> SetStatus(Guid id, string status);
        Task<bool> HasApplicationOnCategory(Guid appCategoryId);
        Task<ServiceApplicationViewModel> UpdateStatus(ServiceApplicationUpdateStatusViewModel model);
        Task<CountViewModel> CountApps(Guid partyId, string role);
        Task<bool> GetAffiliateByAppId(Guid appId);
    }
}
