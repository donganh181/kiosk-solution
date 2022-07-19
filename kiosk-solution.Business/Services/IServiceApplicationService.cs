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
        public Task<ServiceApplicationViewModel> UpdateInformation(Guid updaterId, UpdateServiceApplicationViewModel model);
        public Task<DynamicModelResponse<ServiceApplicationSearchViewModel>> GetAllWithPaging(string role, Guid? id, ServiceApplicationSearchViewModel model, int size, int pageNum);
        public Task<ServiceApplicationViewModel> Create(Guid partyId, CreateServiceApplicationViewModel model);
        public Task<ServiceApplicationViewModel> GetById(Guid id);
        public Task<bool> SetStatus(Guid id, string status);
        public Task<bool> HasApplicationOnCategory(Guid appCategoryId);

        public Task<ServiceApplicationViewModel> UpdateStatus(ServiceApplicationUpdateStatusViewModel model);
    }
}
