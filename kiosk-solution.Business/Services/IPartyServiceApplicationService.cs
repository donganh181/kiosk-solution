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
        Task<PartyServiceApplicationViewModel> Create(Guid id, PartyServiceApplicationCreateViewModel model);
        Task<DynamicModelResponse<PartyServiceApplicationSearchViewModel>> GetAllWithPaging(Guid id, PartyServiceApplicationSearchViewModel model, int size, int pageNum);
        Task<bool> CheckAppExist(Guid partyId, Guid cateId);
        Task<bool> CheckAppExistByPartyIdAndServiceApplicationId(Guid partyId, Guid serviceApplicationId);
        Task<PartyServiceApplicationViewModel> UpdateStatus(Guid partyId, PartyServiceApplicationUpdateViewModel model);
        Task<int> CountUserByAppId(Guid appId);
        Task<List<MyAppViewModel>> GetListAppByPartyId(Guid partyId);
        Task<List<dynamic>> GetListAppByTemplateId(Guid templateId);
        Task<List<dynamic>> GetListAppByAppcategoryIdAndPartyId(Guid cateId, Guid partyId);
    }
}
