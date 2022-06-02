﻿using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.AutoMapper
{
    public static class ServiceApplicationModule
    {
        public static void ConfigServiceApplicationModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<ServiceApplication, ServiceApplicationViewModel>();
            mc.CreateMap<ServiceApplicationViewModel, ServiceApplication>();

            mc.CreateMap<ServiceApplication, ServiceApplicationSearchViewModel>().ForMember(src => src.PartyName, opt => opt.MapFrom(des => des.Party.FirstName))
                .ForMember(src => src.AppCategoryName, opt => opt.MapFrom(des => des.AppCategory.Name));
            mc.CreateMap<ServiceApplicationSearchViewModel, ServiceApplication>();
            mc.CreateMap<ServiceApplication, CreateServiceApplicationViewModel>();
            mc.CreateMap<CreateServiceApplicationViewModel, ServiceApplication>();
        }
    }
}
