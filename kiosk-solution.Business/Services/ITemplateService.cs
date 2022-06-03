using kiosk_solution.Data.ViewModels;
using System;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services
{
    public interface ITemplateService
    {
        Task<bool> IsOwner(Guid partyId, Guid templateId);
        Task<TemplateViewModel> Create(Guid id, TemplateCreateViewModel model);
    }
}