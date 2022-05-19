using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Business.Services
{
    public interface IRoleService
    {
        Task<string> GetRoleNameById(Guid id);
        Task<Guid> GetIdByRoleName(string roleName);
        Task<List<RoleViewModel>> GetAll();
    }
}