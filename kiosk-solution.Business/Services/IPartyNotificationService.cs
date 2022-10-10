using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services
{
    public interface IPartyNotificationService
    {
        public Task<PartyNotificationViewModel> Create(PartyNotificationCreateViewModel model);
        public Task<NotificationDynamicModelResponse<PartyNotificationViewModel>> Get(Guid partyId, int size, int pageNum);
        public Task<PartyNotificationViewModel> GetById(Guid partyId,Guid partyNotiId);
    }
}
