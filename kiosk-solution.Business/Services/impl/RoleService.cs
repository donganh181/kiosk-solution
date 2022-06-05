using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Google.Apis.Logging;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace kiosk_solution.Business.Services.impl
{
    public class RoleService :IRoleService
    {
        private readonly AutoMapper.IConfigurationProvider _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IRoleService> _logger;
        public RoleService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, ILogger<IRoleService> logger) 
        {
            _mapper = mapper.ConfigurationProvider;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<string> GetRoleNameById(Guid id)
        {
            var role = await _unitOfWork.RoleRepository.Get(r => r.Id.Equals(id)).FirstOrDefaultAsync();

            return role.Name;
        }

        public async Task<Guid> GetIdByRoleName(string roleName)
        {
            var role = await _unitOfWork.RoleRepository.Get(r => r.Name == roleName).FirstOrDefaultAsync();
            return role.Id;
        }

        public async Task<List<RoleViewModel>> GetAll()
        {
            var list = await _unitOfWork.RoleRepository.Get().ProjectTo<RoleViewModel>(_mapper).ToListAsync();
            if(list == null)
            {
                _logger.LogInformation("Not Found");
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.");
            }
            return list;
        }
    }
}
