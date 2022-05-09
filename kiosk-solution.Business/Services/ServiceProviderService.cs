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
    public interface IServiceProviderService : IBaseService<TblServiceProvider>
    {
        Task<SuccessResponse<ServiceProviderLoginSuccessViewModel>> Login(LoginViewModel model);
    }
    public class ServiceProviderService : BaseService<TblServiceProvider>, IServiceProviderService
    {
        private readonly AutoMapper.IConfigurationProvider _mapper;
        private readonly IConfiguration _configuration;

        public ServiceProviderService(IServiceProviderRepository repository, IMapper mapper, IConfiguration configuration) : base(repository)
        {
            _mapper = mapper.ConfigurationProvider;
            _configuration = configuration;
        }
        public async Task<SuccessResponse<ServiceProviderLoginSuccessViewModel>> Login(LoginViewModel model)
        {
            var provider = await Get(p => p.Email.Equals(model.email) && p.Password.Equals(model.password)).ProjectTo<ServiceProviderViewModel>(_mapper).FirstOrDefaultAsync();

            if (provider == null) throw new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.");

            if (provider.Status.Equals(AccountStatusConstants.DEACTIVATE)) throw new ErrorResponse((int)HttpStatusCode.Forbidden, "This user has been deactivate");

            string token = TokenUtil.GenerateServiceProviderJWTWebToken(provider, _configuration);

            var result = _mapper.CreateMapper().Map<ServiceProviderLoginSuccessViewModel>(provider);
            if (result.Password.Equals(DefaultConstants.DEFAULT_PASSWORD))
            {
                result.PasswordIsChanged = false;
            }
            else
            {
                result.PasswordIsChanged = true;
            }

            result.Token = token;

            return new SuccessResponse<ServiceProviderLoginSuccessViewModel>(200, "Login Success", result);

        }
    }
}
