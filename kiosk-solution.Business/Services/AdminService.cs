using kiosk_solution.Business.Utilities;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace kiosk_solution.Business.Services
{
    public interface IAdminService : IBaseService<TblAdmin>
    {
        Task<SuccessResponse<AdminLoginSuccessViewModel>> Login(LoginViewModel model);
    }
    public class AdminService : BaseService<TblAdmin>, IAdminService
    {
        private readonly AutoMapper.IConfigurationProvider _mapper;
        private readonly IConfiguration _configuration;

        public AdminService( IAdminRepository repository, IMapper mapper, IConfiguration configuration) : base(repository)
        {
            _mapper = mapper.ConfigurationProvider;
            _configuration = configuration;
        }

        public async Task<SuccessResponse<AdminLoginSuccessViewModel>> Login(LoginViewModel model)
        {
            var admin = await Get(a => a.Email.Equals(model.email) && a.Password.Equals(model.password)).ProjectTo<AdminViewModel>(_mapper).FirstOrDefaultAsync();

            if(admin == null) throw new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.");

            if(admin.Status.Equals(AccountStatusConstants.DEACTIVATE)) throw new ErrorResponse((int)HttpStatusCode.Forbidden, "This user has been deactivate");

            string token = TokenUtil.GenerateAdminJWTWebToken(admin, _configuration);

            var result = _mapper.CreateMapper().Map<AdminLoginSuccessViewModel>(admin);

            result.Token = token;

            return new SuccessResponse<AdminLoginSuccessViewModel>(200, "Login Success", result)
            {
            };

        }
    }
}
