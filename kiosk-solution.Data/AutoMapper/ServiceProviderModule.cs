using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace kiosk_solution.Data.AutoMapper
{
    public static class ServiceProviderModule
    {
        public static void ConfigServiceProviderModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<ServiceProviderViewModel, TblServiceProvider>();
            mc.CreateMap<TblServiceProvider, ServiceProviderViewModel>();

            mc.CreateMap<ServiceProviderViewModel, ServiceProviderLoginSuccessViewModel>();
            mc.CreateMap<ServiceProviderLoginSuccessViewModel, ServiceProviderViewModel>();
        }
    }
}
