using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services
{
    public interface IServiceApplicationPublishRequestService
    {
        public Task<ServiceApplicationPublishRequestViewModel> Create(Guid creatorId, ServiceApplicationPublishRequestCreateViewModel model);

        public Task<ServiceApplicationPublishRequestViewModel> Update(Guid handlerId,
            UpdateServiceApplicationPublishRequestViewModel model);
    }
}
