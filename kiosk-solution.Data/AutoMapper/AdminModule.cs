using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace kiosk_solution.Data.AutoMapper
{
    public static class AdminModule
    {
        public static void ConfigAdminModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<AdminViewModel, TblAdmin>();
            mc.CreateMap<TblAdmin, AdminViewModel>();

            mc.CreateMap<AdminViewModel, AdminLoginSuccessViewModel>();
            mc.CreateMap<AdminLoginSuccessViewModel, AdminViewModel>();
        }
    }
}
