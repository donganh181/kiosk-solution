using System;
using System.Collections.Generic;
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
using BCryptNet = BCrypt.Net.BCrypt;

namespace kiosk_solution.Business.Services.impl
{
    
    public class PartyService : IPartyService
    {
        private readonly AutoMapper.IConfigurationProvider _mapper;
        private readonly IConfiguration _configuration;
        private readonly IRoleService _roleService;
        private readonly IUnitOfWork _unitOfWork;

        public PartyService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IRoleService roleService)
        {
            _mapper = mapper.ConfigurationProvider;
            _configuration = configuration;
            _roleService = roleService;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PartyViewModel>> GetAll()
        {
            var list = await _unitOfWork.PartyRepository.Get().ProjectTo<PartyViewModel>(_mapper).ToListAsync();
            if(list == null) throw new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.");

            return list;
        }

        public async Task<PartyViewModel> Login(LoginViewModel model)
        {
            var user = await _unitOfWork.PartyRepository.Get(u => u.Email.Equals(model.email)).ProjectTo<PartyViewModel>(_mapper).FirstOrDefaultAsync();

            if (user == null || !BCryptNet.Verify(model.password, user.Password))
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.");
            if (user.Status.Equals(AccountStatusConstants.DEACTIVATE))
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "This user has been banned.");

            var roleName = await _roleService.GetRoleNameById(Guid.Parse(user.RoleId.ToString()));
            user.RoleName = roleName;
            string token = TokenUtil.GenerateJWTWebToken(user, _configuration);
            var result = _mapper.CreateMapper().Map<PartyViewModel>(user);

            result.Token = token;

            if(BCryptNet.Verify(DefaultConstants.DEFAULT_PASSWORD, result.Password))
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
                await EmailUtil.SendCreateAccountEmail(account.Email);
                var result = _mapper.CreateMapper().Map<PartyViewModel>(account);
                return result;
            }
            catch (DbUpdateException) {
                throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid Data");
            }
        }

        public async Task<PartyViewModel> UpdateAccount(Guid accountId, UpdateAccountViewModel model)
        {
            var updater = await _unitOfWork.PartyRepository.Get(u => u.Id.Equals(accountId)).FirstOrDefaultAsync();
            if (updater == null) throw new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.");

            var updaterRoleName = await _roleService.GetRoleNameById(Guid.Parse(updater.RoleId.ToString()));
            if (updaterRoleName.Equals("Admin") || updater.Id.Equals(model.Id))
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
                    throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid Data");
                }
            }else throw new ErrorResponse((int)HttpStatusCode.Forbidden, "Your account cannot use this feature.");
        }

        public async Task<PartyViewModel> UpdatePassword(Guid id, UpdatePasswordViewModel model)
        {
            var user = await _unitOfWork.PartyRepository.Get(u => u.Id.Equals(id)).FirstOrDefaultAsync();
            if (!BCrypt.Net.BCrypt.Verify(model.OldPasssword, user.Password))
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Wrong old password");
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
                throw new ErrorResponse((int) HttpStatusCode.UnprocessableEntity, "Invalid Data");
            }

        }
    }
}
