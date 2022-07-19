using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services
{
    public interface IServiceApplicationFeedBackService
    {
        public Task<ServiceApplicationFeedBackViewModel> Create(Guid partyId, ServiceApplicationFeedBackCreateViewModel model);
        public Task<ServiceApplicationFeedBackViewModel> Update(Guid partyId, ServiceApplicationFeedBackUpdateViewModel model);
        public Task<DynamicModelResponse<ServiceApplicationFeedBackViewModel>> GetListFeedbackByAppId(Guid appId, int size, int pageNum);
        public Task<ServiceApplicationFeedBackViewModel> GetFeedbackById(Guid id);
    }
}
