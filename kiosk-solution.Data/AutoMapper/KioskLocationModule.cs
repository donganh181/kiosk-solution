using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.AutoMapper
{
    public static class KioskLocationModule
    {
        public static void ConfigKioskLocationModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<KioskLocation, KioskLocationViewModel>()
                     .ForMember(src => src.OwnerEmail, opt => opt.MapFrom(des => des.Owner.Email));
            mc.CreateMap<KioskLocationViewModel, KioskLocation>();

            mc.CreateMap<KioskLocation, CreateKioskLocationViewModel>();
            mc.CreateMap<CreateKioskLocationViewModel, KioskLocation>();

            mc.CreateMap<KioskLocation, KioskLocationSearchViewModel>()
                .ForMember(src => src.OwnerEmail, opt => opt.MapFrom(des => des.Owner.Email));
            mc.CreateMap<KioskLocationSearchViewModel, KioskLocation>();
        }
    }
}
