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
    public static class ServiceApplicationPublishRequestModule
    {
        public static void ConfigServiceApplicationPublishRequestModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<ServiceApplicationPublishRequest, ServiceApplicationPublishRequestViewModel>();
            mc.CreateMap<ServiceApplicationPublishRequestViewModel, ServiceApplicationPublishRequest>();

            mc.CreateMap<ServiceApplicationPublishRequest, ServiceApplicationPublishRequestCreateViewModel>();
            mc.CreateMap<ServiceApplicationPublishRequestCreateViewModel, ServiceApplicationPublishRequest>();

            mc.CreateMap<ServiceApplicationPublishRequest, UpdateServiceApplicationPublishRequestViewModel>();
            mc.CreateMap<UpdateServiceApplicationPublishRequestViewModel, ServiceApplicationPublishRequest>();
        }
    }
}
