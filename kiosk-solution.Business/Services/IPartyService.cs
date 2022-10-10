using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Services
{
    public interface IPartyService
    {
        Task<PartyViewModel> Login(LoginViewModel model);
        Task<PartyViewModel> CreateAccount(Guid creatorId, CreateAccountViewModel model);
        Task<PartyViewModel> UpdateAccount(Guid accountId, UpdateAccountViewModel model);
        Task<PartyViewModel> UpdatePassword(Guid id, UpdatePasswordViewModel model);
        Task<PartyViewModel> UpdateStatus(Guid id);
        Task<DynamicModelResponse<PartySearchViewModel>> GetAllWithPaging(PartySearchViewModel model, int size, int pageNum);
        Task<PartyViewModel> GetPartyById(Guid id, string roleName, Guid? checkId);
        Task<bool> Logout(Guid partyId);
        Task<PartyByKioskIdViewModel> GetPartyByKioskId(Guid id);
        Task<bool> ForgetPassword(string email);
        Task<PartyResetPasswordViewModel> ResetPassword(Guid partyId, string verifyCode);

        Task<CountViewModel> CountLocationOwner();

        Task<CountViewModel> CountServiceProvider();
    }
}