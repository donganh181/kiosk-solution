using AutoMapper;
using BCryptNet = BCrypt.Net.BCrypt;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kiosk_solution.Data.Responses;
using System.Net;
using kiosk_solution.Data.Constants;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Business.Utilities;

namespace kiosk_solution.Business.Services
{
    public interface IPartyService : IBaseService<Party>
    {
        Task<SuccessResponse<PartyViewModel>> Login(LoginViewModel model);
        Task<SuccessResponse<PartyViewModel>> CreateAccount(Guid creatorId, CreateAccountViewModel model);
        Task<List<PartyViewModel>> GetAll();
    }
    public class PartyService : BaseService<Party>, IPartyService
    {
        private readonly AutoMapper.IConfigurationProvider _mapper;
        private readonly IConfiguration _configuration;

        private readonly IRoleService _roleService;

        public PartyService(IPartyRepository repository, IMapper mapper, IConfiguration configuration, IRoleService roleService) : base(repository)
        {
            _mapper = mapper.ConfigurationProvider;
            _configuration = configuration;
            _roleService = roleService;
        }

        public async Task<List<PartyViewModel>> GetAll()
        {
            return await Get().ProjectTo<PartyViewModel>(_mapper).ToListAsync();
        }

        public async Task<SuccessResponse<PartyViewModel>> Login(LoginViewModel model)
        {
            var user = await Get(u => u.Email.Equals(model.email)).ProjectTo<PartyViewModel>(_mapper).FirstOrDefaultAsync();

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

            return new SuccessResponse<PartyViewModel>(200, "Login Success", result)
            {
            };
        }
        
        public async Task<SuccessResponse<PartyViewModel>> CreateAccount(Guid creatorId, CreateAccountViewModel model)
        {
            var account = _mapper.CreateMapper().Map<Party>(model);
            account.Password = DefaultConstants.DEFAULT_PASSWORD;
            account.CreatorId = creatorId;
            account.Status = AccountStatusConstants.ACTIVE;
            account.RoleId = await _roleService.GetIdByRoleName(model.roleName);
            account.CreateDate = DateTime.Now;
            try
            {
                await CreateAsync(account);
                var result = _mapper.CreateMapper().Map<PartyViewModel>(account);
                return new SuccessResponse<PartyViewModel>(200, "Create success", result);       
            }
            catch (Exception) {
                throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid Data");
            }
        }
    }
}
