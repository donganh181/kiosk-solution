using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace kiosk_solution.Data.AutoMapper
{
    public static class LocationOwnerModule
    {
        public static void ConfigLocationOwnerModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<LocationOwnerViewModel, TblLocationOwner>();
            mc.CreateMap<TblLocationOwner, LocationOwnerViewModel>();

            mc.CreateMap<LocationOwnerViewModel, LocationOwnerLoginSuccessViewModel>();
            mc.CreateMap<LocationOwnerLoginSuccessViewModel, LocationOwnerViewModel>();
        }
    }
}
