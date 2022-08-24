using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Business.Utilities;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BCryptNet = BCrypt.Net.BCrypt;

namespace kiosk_solution.Business.Services.impl
{
    public class PartyService : IPartyService
    {
        private readonly AutoMapper.IConfigurationProvider _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IPartyService> _logger;

        public PartyService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration,
            ILogger<IPartyService> logger)
        {
            _mapper = mapper.ConfigurationProvider;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<PartyViewModel> Login(LoginViewModel model)
        {
            var user = await _unitOfWork.PartyRepository
                .Get(u => u.Email.Equals(model.Email))
                .FirstOrDefaultAsync();

            if (user == null || !BCryptNet.Verify(model.Password, user.Password))
            {
                _logger.LogInformation("Not Found");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Not found.");
            }

            if (user.Status.Equals(StatusConstants.DEACTIVATE))
            {
                _logger.LogInformation($"{model.Email} has been banned.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "This user has been banned.");
            }

            try
            {
                user.DeviceId = model.DeviceId;
                _unitOfWork.PartyRepository.Update(user);
                await _unitOfWork.SaveAsync();

                var result = await _unitOfWork.PartyRepository
                    .Get(u => u.Email.Equals(model.Email))
                    .Include(u => u.Role)
                    .Include(u => u.Creator)
                    .ProjectTo<PartyViewModel>(_mapper).FirstOrDefaultAsync();

                string token = TokenUtil.GenerateJWTWebToken(result, _configuration);

                result.Token = token;

                if (BCryptNet.Verify(DefaultConstants.DEFAULT_PASSWORD, result.Password))
                {
                    result.PasswordIsChanged = false;
                }
                else
                {
                    result.PasswordIsChanged = true;
                }

                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<PartyViewModel> CreateAccount(Guid creatorId, CreateAccountViewModel model)
        {
            var account = _mapper.CreateMapper().Map<Party>(model);
            account.Password = BCrypt.Net.BCrypt.HashPassword(DefaultConstants.DEFAULT_PASSWORD);
            account.CreatorId = creatorId;
            account.Status = StatusConstants.ACTIVATE;
            account.CreateDate = DateTime.Now;

            try
            {
                await _unitOfWork.PartyRepository.InsertAsync(account);
                await _unitOfWork.SaveAsync();

                string subject = EmailConstants.CREATE_ACCOUNT_SUBJECT;
                string content = EmailUtil.getCreateAccountContent(account.Email);
                await EmailUtil.SendEmail(account.Email, subject, content);

                var result = _mapper.CreateMapper().Map<PartyViewModel>(account);
                return result;
            }
            catch (Exception e)
            {
                if (e.InnerException.Message.Contains("Cannot insert duplicate key"))
                {
                    _logger.LogInformation("Phone or Email is duplicated.");
                    throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Phone or Email is duplicated.");
                }
                else
                {
                    _logger.LogInformation("Invalid data.");
                    throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid data.");
                }
            }
        }

        public async Task<PartyViewModel> UpdateAccount(Guid accountId, UpdateAccountViewModel model)
        {
            var updater = await _unitOfWork.PartyRepository.Get(u => u.Id.Equals(accountId)).Include(u => u.Role)
                .FirstOrDefaultAsync();

            if (updater.Role.Name.Equals("Admin") || updater.Id.Equals(model.Id))
            {
                var user = await _unitOfWork.PartyRepository.Get(us => us.Id.Equals(model.Id)).FirstOrDefaultAsync();

                if (user == null) throw new ErrorResponse((int) HttpStatusCode.NotFound, "Not found.");
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;
                user.DateOfBirth = model.DateOfBirth;
                try
                {
                    _unitOfWork.PartyRepository.Update(user);
                    await _unitOfWork.SaveAsync();
                    var result = _mapper.CreateMapper().Map<PartyViewModel>(user);
                    return result;
                }
                catch (Exception)
                {
                    _logger.LogInformation("Invalid Data.");
                    throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid Data.");
                }
            }
            else
            {
                _logger.LogInformation($"account {updater.Email} cannot use this feature.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }
        }

        public async Task<PartyViewModel> UpdatePassword(Guid id, UpdatePasswordViewModel model)
        {
            var user = await _unitOfWork.PartyRepository.Get(u => u.Id.Equals(id)).FirstOrDefaultAsync();
            if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, user.Password))
            {
                _logger.LogInformation("Wrong old password");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Wrong old password");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            try
            {
                _unitOfWork.PartyRepository.Update(user);
                await _unitOfWork.SaveAsync();
                var result = _mapper.CreateMapper().Map<PartyViewModel>(user);
                result.PasswordIsChanged = true;
                return result;
            }
            catch (DbUpdateException)
            {
                _logger.LogInformation("Invalid Data");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid Data");
            }
        }

        public async Task<PartyViewModel> UpdateStatus(Guid id)
        {
            var user = await _unitOfWork.PartyRepository.Get(u => u.Id.Equals(id)).Include(u => u.Role)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                _logger.LogInformation("Not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Not found.");
            }

            if (user.Role.Name.Equals(RoleConstants.ADMIN))
            {
                _logger.LogInformation($"{user.Email} cannot change status of admin.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }

            if (user.Status.Equals(StatusConstants.ACTIVATE))
                user.Status = StatusConstants.DEACTIVATE;
            else
                user.Status = StatusConstants.ACTIVATE;
            try
            {
                _unitOfWork.PartyRepository.Update(user);
                await _unitOfWork.SaveAsync();

                string subject = EmailUtil.getUpdateStatusSubject(user.Status.Equals(StatusConstants.ACTIVATE));
                string content =
                    EmailUtil.getUpdateStatusContent(user.Email, user.Status.Equals(StatusConstants.ACTIVATE));
                await EmailUtil.SendEmail(user.Email, subject, content);

                var result = _mapper.CreateMapper().Map<PartyViewModel>(user);
                return result;
            }
            catch (DbUpdateException)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<DynamicModelResponse<PartySearchViewModel>> GetAllWithPaging(PartySearchViewModel model,
            int size, int pageNum)
        {
            var users = _unitOfWork.PartyRepository.Get().Include(u => u.Role).Include(u => u.Creator)
                .ProjectTo<PartySearchViewModel>(_mapper)
                .DynamicFilter(model)
                .AsQueryable().OrderByDescending(r => r.CreateDate).ThenByDescending(r => r.Email);

            var listPaging = users
                .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);

            if (listPaging.Data.ToList().Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not Found");
            }

            var result = new DynamicModelResponse<PartySearchViewModel>
            {
                Metadata = new PagingMetaData
                {
                    Page = pageNum,
                    Size = size,
                    Total = listPaging.Total
                },
                Data = listPaging.Data.ToList()
            };
            return result;
        }

        public async Task<PartyViewModel> GetPartyById(Guid id, string roleName, Guid? checkId)
        {
            PartyViewModel party = null;
            if (roleName.Equals(RoleConstants.ADMIN) || roleName.Equals(RoleConstants.SYSTEM))
            {
                party = await _unitOfWork.PartyRepository
                    .Get(p => p.Id.Equals(id))
                    .Include(p => p.Role)
                    .Include(p => p.Creator)
                    .ProjectTo<PartyViewModel>(_mapper).FirstOrDefaultAsync();
            }
            else if (roleName.Equals(RoleConstants.LOCATION_OWNER)||roleName.Equals(RoleConstants.SERVICE_PROVIDER))
            {
                if (!id.Equals(checkId))
                {
                    _logger.LogInformation("You can not get another user info.");
                    throw new ErrorResponse((int) HttpStatusCode.BadRequest, "You can not get another user info.");
                }

                party = await _unitOfWork.PartyRepository
                    .Get(p => p.Id.Equals(id))
                    .Include(p => p.Role)
                    .Include(p => p.Creator)
                    .ProjectTo<PartyViewModel>(_mapper).FirstOrDefaultAsync();
            }
            else
            {
                _logger.LogInformation("Cannot access.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "Cannot access.");
            }

            if (party == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not found.");
            }

            if (BCryptNet.Verify(DefaultConstants.DEFAULT_PASSWORD, party.Password))
            {
                party.PasswordIsChanged = false;
            }
            else
            {
                party.PasswordIsChanged = true;
            }

            return party;
        }

        public async Task<bool> Logout(Guid partyId)
        {
            var user = await _unitOfWork.PartyRepository
                .Get(u => u.Id.Equals(partyId))
                .FirstOrDefaultAsync();

            if (user == null)
            {
                _logger.LogInformation("Not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Not found.");
            }

            user.DeviceId = string.Empty;

            try
            {
                _unitOfWork.PartyRepository.Update(user);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Invalid Data.");
            }
        }

        public async Task<PartyByKioskIdViewModel> GetPartyByKioskId(Guid id)
        {
            var party = await _unitOfWork.PartyRepository
                .Get(p => p.Role.Name.Equals(RoleConstants.LOCATION_OWNER))
                .Include(a => a.Kiosks)
                .ToListAsync();

            if (party.Count < 1)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not found.");
            }

            foreach (var item in party)
            {
                foreach (var kiosk in item.Kiosks)
                {
                    if (kiosk.Id.Equals(id))
                    {
                        var result = _mapper.CreateMapper().Map<PartyByKioskIdViewModel>(item);
                        return result;
                    }
                }
            }

            _logger.LogInformation("Can not Found.");
            throw new ErrorResponse((int) HttpStatusCode.NotFound, "Can not found.");
        }

        public async Task<bool> ForgetPassword(string email)
        {
            var acc = await _unitOfWork.PartyRepository.Get(p => p.Email.Equals(email)).FirstOrDefaultAsync();
            if (acc == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Can not found.");
            }

            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            var length = 10;
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }

            acc.VerifyCode = res + "-" + DateTime.Now;

            try
            {
                _unitOfWork.PartyRepository.Update(acc);
                await _unitOfWork.SaveAsync();

                string subject = EmailConstants.FORGET_PASSWORD_SUBJECT;
                string link = EmailConstants.FORGET_PASSWORD_LINK;
                link = link.Replace("PARTY_ID", acc.Id.ToString());
                link = link.Replace("VERIFY_CODE", acc.VerifyCode);
                string content = EmailUtil.GetForgetPasswordContent(link);
                await EmailUtil.SendEmail(acc.Email, subject, content);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, e.Message);
            }
        }

        public async Task<PartyResetPasswordViewModel> ResetPassword(Guid partyId, string verifyCode)
        {
            var acc = await _unitOfWork.PartyRepository.Get(p => p.Id.Equals(partyId) && p.VerifyCode != null)
                .FirstOrDefaultAsync();
            if (acc == null)
            {
                _logger.LogInformation("Can not Found.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Can not found.");
            }

            if (!acc.VerifyCode.Split("-")[0].Equals(verifyCode))
            {
                _logger.LogInformation("Wrong verify code.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Wrong verify code.");
            }

            var timeCode = DateTime.Parse(acc.VerifyCode.Split("-")[1]);
            var timeNow = DateTime.Now;
            if (DateTime.Compare(timeCode.AddMinutes(5), timeNow) < 0)
            {
                _logger.LogInformation("Code has expired.");
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Code has expired, please get another code and try again.");
            }
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            var length = 7;
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }

            var newPassword = res.ToString();
            acc.VerifyCode = null;
            acc.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            try
            {
                _unitOfWork.PartyRepository.Update(acc);
                await _unitOfWork.SaveAsync();
                string subject = EmailConstants.RESET_PASSWORD_SUBJECT;
                string content = EmailUtil.GetResetPasswordContent(newPassword);
                await EmailUtil.SendEmail(acc.Email, subject, content);
                return new PartyResetPasswordViewModel()
                {
                    newPassword = newPassword
                };
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, e.Message);
            }
        }

        public async Task<CountViewModel> CountLocationOwner()
        {
            return new CountViewModel()
            {
                total = await _unitOfWork.PartyRepository.Get(e => e.Role.Name.Equals(RoleConstants.LOCATION_OWNER))
                    .CountAsync(),
                active = await _unitOfWork.PartyRepository.Get(e =>
                        e.Role.Name.Equals(RoleConstants.LOCATION_OWNER) && e.Status.Contains(StatusConstants.ACTIVATE))
                    .CountAsync(),
                deactive = await _unitOfWork.PartyRepository.Get(e =>
                        e.Role.Name.Equals(RoleConstants.LOCATION_OWNER) &&
                        e.Status.Contains(StatusConstants.DEACTIVATE))
                    .CountAsync(),
            };
        }

        public async Task<CountViewModel> CountServiceProvider()
        {
            return new CountViewModel()
            {
                total = await _unitOfWork.PartyRepository.Get(e => e.Role.Name.Equals(RoleConstants.SERVICE_PROVIDER))
                    .CountAsync(),
                active = await _unitOfWork.PartyRepository.Get(e =>
                        e.Role.Name.Equals(RoleConstants.SERVICE_PROVIDER) &&
                        e.Status.Contains(StatusConstants.ACTIVATE))
                    .CountAsync(),
                deactive = await _unitOfWork.PartyRepository.Get(e =>
                        e.Role.Name.Equals(RoleConstants.SERVICE_PROVIDER) &&
                        e.Status.Contains(StatusConstants.DEACTIVATE))
                    .CountAsync(),
            };
        }
    }
}