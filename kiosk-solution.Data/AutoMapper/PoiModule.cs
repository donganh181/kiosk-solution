using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Data.AutoMapper
{
    public static class PoiModule
    {
        public static void ConfigPoiModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Poi, PoiCreateViewModel>();
            mc.CreateMap<PoiCreateViewModel, Poi>();

            mc.CreateMap<Poi, PoiViewModel>();
            mc.CreateMap<PoiViewModel, Poi>();

        }
    }
}