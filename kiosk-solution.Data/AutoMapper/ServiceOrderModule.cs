using System.Security.Cryptography;
using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Data.AutoMapper
{
    public static class ServiceOrderModule
    {
        public static void ConfigServiceOrderModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<ServiceOrder, ServiceOrderCreateViewModel>();
            mc.CreateMap<ServiceOrderCreateViewModel, ServiceOrder>();

            mc.CreateMap<ServiceOrder, ServiceOrderViewModel>()
                .ForMember(src => src.KioskName, opt => opt.MapFrom(des => des.Kiosk.Name));
            mc.CreateMap<ServiceOrderViewModel, ServiceOrder>();

            mc.CreateMap<ServiceOrder, ServiceOrderSearchViewModel>();
            mc.CreateMap<ServiceOrderSearchViewModel, ServiceOrder>();

            mc.CreateMap<ServiceOrder, ServiceOrderCommissionSearchViewModel>()
                .ForMember(src => src.ServiceApplicationName, opt => opt.MapFrom(des => des.ServiceApplication.Name))
                .ForMember(src => src.KioskName, opt => opt.MapFrom(des => des.Kiosk.Name));
            mc.CreateMap<ServiceOrderCommissionSearchViewModel, ServiceOrder>();

            mc.CreateMap<ServiceOrder, ServiceOrderLocationOwnerViewModel>()
                .ForMember(src => src.KioskName, opt => opt.MapFrom(des => des.Kiosk.Name));
            mc.CreateMap<ServiceOrderLocationOwnerViewModel, ServiceOrder>();
        }
    }
}