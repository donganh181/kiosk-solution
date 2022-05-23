﻿using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Data.AutoMapper
{
    public static class KioskModule
    {
        public static void ConfigKioskModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Kiosk, KioskViewModel>();
            mc.CreateMap<KioskViewModel, Kiosk>();
        }
    }
}