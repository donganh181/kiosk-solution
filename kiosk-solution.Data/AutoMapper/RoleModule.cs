using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Data.AutoMapper
{
    public static class RoleModule
    {
        public static void ConfigRoleModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Role, RoleViewModel>();
            mc.CreateMap<RoleViewModel, Role>();
        }
    }
}