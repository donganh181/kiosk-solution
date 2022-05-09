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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services
{
    public interface ILocationOwnerService : IBaseService<TblLocationOwner>
    {
        Task<SuccessResponse<LocationOwnerLoginSuccessViewModel>> Login(LoginViewModel model);
    }
    public class LocationOwnerService : BaseService<TblLocationOwner>, ILocationOwnerService
    {
        private readonly AutoMapper.IConfigurationProvider _mapper;
        private readonly IConfiguration _configuration;

        public LocationOwnerService(ILocationOwnerRepository repository, IMapper mapper, IConfiguration configuration) : base(repository)
        {
            _mapper = mapper.ConfigurationProvider;
            _configuration = configuration;
        }
        public async Task<SuccessResponse<LocationOwnerLoginSuccessViewModel>> Login(LoginViewModel model)
        {
            var owner = await Get(o => o.Email.Equals(model.email) && o.Password.Equals(model.password)).ProjectTo<LocationOwnerViewModel>(_mapper).FirstOrDefaultAsync();

            if(owner == null) throw new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.");

            if(owner.Status.Equals(AccountStatusConstants.DEACTIVATE)) throw new ErrorResponse((int)HttpStatusCode.Forbidden, "This user has been deactivate");

            string token = TokenUtil.GenerateLocationOwnerJWTWebToken(owner, _configuration);

            var result = _mapper.CreateMapper().Map<LocationOwnerLoginSuccessViewModel>(owner);
            if (result.Password.Equals(DefaultConstants.DEFAULT_PASSWORD))
            {
                result.PasswordIsChanged = false;
            }
            else
            {
                result.PasswordIsChanged = true;
            }

            result.Token = token;

            return new SuccessResponse<LocationOwnerLoginSuccessViewModel>(200, "Login Success", result);
        }
    }
}
