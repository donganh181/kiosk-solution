using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;
using System.Linq;

namespace kiosk_solution.Data.AutoMapper
{
    public static class KioskModule
    {
        public static void ConfigKioskModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Kiosk, KioskViewModel>();
            mc.CreateMap<KioskViewModel, Kiosk>();

            mc.CreateMap<Kiosk, KioskNearbyViewModel>();
            mc.CreateMap<KioskNearbyViewModel, Kiosk>();

            mc.CreateMap<Kiosk, CreateKioskViewModel>();
            mc.CreateMap<CreateKioskViewModel, Kiosk>();

            mc.CreateMap<Kiosk, KioskSearchViewModel>();
            mc.CreateMap<KioskSearchViewModel, Kiosk>();

            mc.CreateMap<Kiosk, KioskDetailViewModel>()
                .ForMember(src => src.KioskScheduleTemplate, opt => opt.MapFrom(des => des.KioskScheduleTemplates.FirstOrDefault()));
            mc.CreateMap<KioskDetailViewModel, Kiosk>();
        }
    }
}