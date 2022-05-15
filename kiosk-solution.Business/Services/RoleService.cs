using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Data.ViewModels;
using kiosk_solution.Data.Responses;
using System.Net;

namespace kiosk_solution.Business.Services
{
    public interface IRoleService : IBaseService<Role>
    {
        Task<string> GetRoleNameById(Guid id);
        Task<Guid> GetIdByRoleName(string roleName);
        Task<List<RoleViewModel>> GetAll();
    }
    public class RoleService : BaseService<Role>, IRoleService
    {
        private readonly AutoMapper.IConfigurationProvider _mapper;
        private readonly IConfiguration _configuration;

        public RoleService(IRoleRepository repository, IMapper mapper, IConfiguration configuration) : base(repository)
        {
            _mapper = mapper.ConfigurationProvider;
            _configuration = configuration;
        }

        public async Task<string> GetRoleNameById(Guid id)
        {
            var role = await Get(r => r.Id.Equals(id)).FirstOrDefaultAsync();

            return role.Name;
        }

        public async Task<Guid> GetIdByRoleName(string roleName)
        {
            var role = await Get(r => r.Name == roleName).FirstOrDefaultAsync();
            return role.Id;
        }

        public async Task<List<RoleViewModel>> GetAll()
        {
            var list = await Get().ProjectTo<RoleViewModel>(_mapper).ToListAsync();
            if(list == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, "Not found.");

            return list;
        }
    }
}
