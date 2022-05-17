using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Services
{
    public interface IPartyService
    {
        Task<PartyViewModel> Login(LoginViewModel model);
        Task<PartyViewModel> CreateAccount(Guid creatorId, CreateAccountViewModel model);
        Task<List<PartyViewModel>> GetAll();
        Task<PartyViewModel> UpdateAccount(Guid accountId, UpdateAccountViewModel model);
        Task<PartyViewModel> UpdatePassword(Guid id, UpdatePasswordViewModel model);
        Task<PartyViewModel> UpdateStatus(Guid id);
    }
}