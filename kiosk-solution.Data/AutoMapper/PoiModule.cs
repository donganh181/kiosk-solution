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

            mc.CreateMap<Poi, PoiViewModel>()
                .ForMember(src => src.CreatorName, opt => opt.MapFrom(des => des.Creator.FirstName))
                .ForMember(src => src.CreatorEmail, opt => opt.MapFrom(des => des.Creator.Email))
                .ForMember(src => src.PoicategoryName, opt => opt.MapFrom(des => des.Poicategory.Name));
            mc.CreateMap<PoiViewModel, Poi>();

            mc.CreateMap<Poi, PoiSearchViewModel>();
            mc.CreateMap<PoiSearchViewModel, Poi>();

            mc.CreateMap<Poi, PoiNearbySearchViewModel>()
                .ForMember(src => src.CreatorName, opt => opt.MapFrom(des => des.Creator.FirstName))
                .ForMember(src => src.CreatorEmail, opt => opt.MapFrom(des => des.Creator.Email))
                .ForMember(src => src.PoicategoryName, opt => opt.MapFrom(des => des.Poicategory.Name));
            mc.CreateMap<PoiNearbySearchViewModel, Poi>();

            mc.CreateMap<Poi, PoiImageViewModel>();
            mc.CreateMap<PoiImageViewModel, Poi>();

        }
    }
}