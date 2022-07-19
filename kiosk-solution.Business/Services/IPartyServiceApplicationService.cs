using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services
{
    public interface IPartyServiceApplicationService
    {
        public Task<PartyServiceApplicationViewModel> Create(Guid id, PartyServiceApplicationCreateViewModel model);
        public Task<DynamicModelResponse<PartyServiceApplicationSearchViewModel>> GetAllWithPaging(Guid id, PartyServiceApplicationSearchViewModel model, int size, int pageNum);
        public Task<bool> CheckAppExist(Guid partyId, Guid cateId);
        public Task<bool> CheckAppExistByPartyIdAndServiceApplicationId(Guid partyId, Guid serviceApplicationId);
        public Task<PartyServiceApplicationViewModel> UpdateStatus(Guid partyId, PartyServiceApplicationUpdateViewModel model);
    }
}
