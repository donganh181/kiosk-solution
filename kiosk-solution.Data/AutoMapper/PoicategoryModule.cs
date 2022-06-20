using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Data.AutoMapper
{
    public static class PoicategoryModule
    {
        public static void ConfigPoicategoryModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Poicategory, PoiCategoryCreateViewModel>();
            mc.CreateMap<PoiCategoryCreateViewModel, Poicategory>();

            mc.CreateMap<Poicategory, PoicategoryViewModel>();
            mc.CreateMap<PoicategoryViewModel, Poicategory>();

            mc.CreateMap<Poicategory, PoiCategorySearchViewModel>();
            mc.CreateMap<PoiCategorySearchViewModel, Poicategory>();
        }
    }
}