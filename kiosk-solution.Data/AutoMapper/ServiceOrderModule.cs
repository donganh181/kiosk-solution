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

            mc.CreateMap<ServiceOrder, ServiceOrderViewModel>();
            mc.CreateMap<ServiceOrderViewModel, ServiceOrder>();

            mc.CreateMap<ServiceOrder, ServiceOrderSearchViewModel>();
            mc.CreateMap<ServiceOrderSearchViewModel, ServiceOrder>();

            mc.CreateMap<ServiceOrder, ServiceOrderCommissionSearchViewModel>().ForMember(
                src => src.serviceApplicationName, opt => opt.MapFrom(des => des.ServiceApplication.Name));
            mc.CreateMap<ServiceOrderCommissionSearchViewModel, ServiceOrder>();
        }
    }
}