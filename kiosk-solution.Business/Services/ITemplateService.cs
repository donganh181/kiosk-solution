using System;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services
{
    public interface ITemplateService
    {
        Task<bool> IsOwner(Guid partyId, Guid templateId);
    }
}