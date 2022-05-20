using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Business.Utilities;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
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

        public PartyService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration,ILogger<IPartyService> logger)
        {
            _mapper = mapper.ConfigurationProvider;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<PartyViewModel> Login(LoginViewModel model)
        {
            var user = await _unitOfWork.PartyRepository.Get(u => u.Email.Equals(model.email)).Include(u => u.Role).ProjectTo<PartyViewModel>(_mapper).FirstOrDefaultAsync();

            if (user == null || !BCryptNet.Verify(model.password, user.Password))
            {
                _logger.LogInformation("Not Found");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.");
            }
                
            if (user.Status.Equals(AccountStatusConstants.DEACTIVATE))
            {
                _logger.LogInformation($"{model.email} has been banned.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "This user has been banned.");
            }
            string token = TokenUtil.GenerateJWTWebToken(user, _configuration);
            var result = _mapper.CreateMapper().Map<PartyViewModel>(user);

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

        public async Task<PartyViewModel> CreateAccount(Guid creatorId, CreateAccountViewModel model)
        {
            var account = _mapper.CreateMapper().Map<Party>(model);
            account.Password = BCrypt.Net.BCrypt.HashPassword(DefaultConstants.DEFAULT_PASSWORD);
            account.CreatorId = creatorId;
            account.Status = AccountStatusConstants.ACTIVE;
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
            catch (DbUpdateException)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int) HttpStatusCode.UnprocessableEntity, "Invalid Data.");
            }
        }

        public async Task<PartyViewModel> UpdateAccount(Guid accountId, UpdateAccountViewModel model)
        {
            var updater = await _unitOfWork.PartyRepository.Get(u => u.Id.Equals(accountId)).Include(u => u.Role).FirstOrDefaultAsync();

            if (updater.Role.Name.Equals("Admin") || updater.Id.Equals(model.Id))
            {
                var user = await _unitOfWork.PartyRepository.Get(us => us.Id.Equals(model.Id)).FirstOrDefaultAsync();

                if (user == null) throw new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.");
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
                    throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid Data.");
                }
            }
            else
            {
                _logger.LogInformation($"account {updater.Email} cannot use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }
            
        }

        public async Task<PartyViewModel> UpdatePassword(Guid id, UpdatePasswordViewModel model)
        {
            var user = await _unitOfWork.PartyRepository.Get(u => u.Id.Equals(id)).FirstOrDefaultAsync();
            if (!BCrypt.Net.BCrypt.Verify(model.OldPasssword, user.Password))
            {
                _logger.LogInformation("Wrong old password");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Wrong old password");
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
                throw new ErrorResponse((int) HttpStatusCode.UnprocessableEntity, "Invalid Data");
            }
        }

        public async Task<PartyViewModel> UpdateStatus(Guid id)
        {
            var user = await _unitOfWork.PartyRepository.Get(u => u.Id.Equals(id)).Include(u => u.Role).FirstOrDefaultAsync();

            if (user == null)
            {
                _logger.LogInformation("Not Found.");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.");
            }

            if (user.Role.Name.Equals(RoleConstants.ADMIN))
            {
                _logger.LogInformation($"{user.Email} cannot change status of admin.");
                throw new ErrorResponse((int) HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }

            if (user.Status.Equals(AccountStatusConstants.ACTIVE))
                user.Status = AccountStatusConstants.DEACTIVATE;
            else
                user.Status = AccountStatusConstants.ACTIVE;
            try
            {
                _unitOfWork.PartyRepository.Update(user);
                await _unitOfWork.SaveAsync();

                string subject = EmailUtil.getUpdateStatusSubject(user.Status.Equals(AccountStatusConstants.ACTIVE));
                string content =
                    EmailUtil.getUpdateStatusContent(user.Email, user.Status.Equals(AccountStatusConstants.ACTIVE));
                await EmailUtil.SendEmail(user.Email, subject, content);

                var result = _mapper.CreateMapper().Map<PartyViewModel>(user);
                return result;
            }
            catch (DbUpdateException)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int) HttpStatusCode.UnprocessableEntity, "Invalid Data.");
            }
        }

        public async Task<DynamicModelResponse<PartySearchViewModel>> GetAllWithPaging(Guid id, PartySearchViewModel model, int size, int pageNum)
        {
            var user = await _unitOfWork.PartyRepository.Get(u => u.Id.Equals(id)).Include(u => u.Role).FirstOrDefaultAsync();
            if (!user.Role.Name.Equals(RoleConstants.ADMIN))
            {
                _logger.LogInformation($"{user.Email} cannot use this feature.");
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "Your account cannot use this feature.");
            }

            var users = _unitOfWork.PartyRepository.Get().Include(u => u.Role).ProjectTo<PartySearchViewModel>(_mapper);
            var listUser = users.ToList();

            if(listUser.Count == 0) throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not found.");
            users = listUser.AsQueryable().OrderByDescending(r => r.LastName).ThenByDescending(r => r.Address);
            var listPaging = users
                    .DynamicFilter(model)
                    .PagingIQueryable(pageNum, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);
            if (listPaging.Item2.ToList().Count<1) throw new ErrorResponse((int)HttpStatusCode.NotFound, "Can not Found");
            var result = new DynamicModelResponse<PartySearchViewModel>
            {
                Metadata = new PagingMetaData
                {
                    Page = pageNum,
                    Size = size,
                    Total = listPaging.Item1
                },
                Data = listPaging.Item2.ToList()
            };
            return result;
        }
    }
}