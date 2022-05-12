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
    }
    public class PartyService : BaseService<Party>, IPartyService
    {
        private readonly AutoMapper.IConfigurationProvider _mapper;
        private readonly IConfiguration _configuration;

        public PartyService(IPartyRepository repository, IMapper mapper, IConfiguration configuration) : base(repository)
        {
            _mapper = mapper.ConfigurationProvider;
            _configuration = configuration;
        }

        public async Task<SuccessResponse<PartyViewModel>> Login(LoginViewModel model)
        {
            var user = await Get(u => u.Email.Equals(model.email)).FirstOrDefaultAsync();

            if (user == null || !BCryptNet.Verify(model.password, user.Password))
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.");
            if(user.Status.Equals(AccountStatusConstants.DEACTIVATE))
                throw new ErrorResponse((int)HttpStatusCode.Forbidden, "This user has been banned.");
            string token = TokenUtil.GenerateJWTWebToken(user, _configuration);
            var result = _mapper.CreateMapper().Map<PartyViewModel>(user);

            result.Token = token;

            return new SuccessResponse<PartyViewModel>(200, "Login Success", result)
            {
            };

        }
    }
}
