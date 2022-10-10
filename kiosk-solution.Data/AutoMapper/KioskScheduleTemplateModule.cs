using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Data.AutoMapper
{
    public static class KioskScheduleTemplateModule
    {
        public static void ConfigKioskScheduleTemplateModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<KioskScheduleTemplate, KioskScheduleTemplateCreateViewModel>();
            mc.CreateMap<KioskScheduleTemplateCreateViewModel, KioskScheduleTemplate>();

            mc.CreateMap<KioskScheduleTemplate, KioskScheduleTemplateViewModel>()
                .ForMember(src => src.Template, opt => opt.MapFrom(des => des.Template))
                .ForMember(src => src.Schedule, opt => opt.MapFrom(des => des.Schedule));
            mc.CreateMap<KioskScheduleTemplateViewModel, KioskScheduleTemplate>();

            mc.CreateMap<KioskScheduleTemplate, KioskScheduleTemplateDetailViewModel>()
                .ForMember(src => src.Template, opt => opt.MapFrom(des => des.Template))
                .ForMember(src => src.Schedule, opt => opt.MapFrom(des => des.Schedule));
            mc.CreateMap<KioskScheduleTemplateDetailViewModel, KioskScheduleTemplate>();

            mc.CreateMap<KioskScheduleTemplate, KioskScheduleTemplateChangeViewModel>()
                .ForMember(src => src.DeviceId, opt => opt.MapFrom(des => des.Kiosk.DeviceId))
                .ForMember(src => src.Template, opt => opt.MapFrom(des => des.Template))
                .ForMember(src => src.Schedule, opt => opt.MapFrom(des => des.Schedule));
            mc.CreateMap<KioskScheduleTemplateChangeViewModel, KioskScheduleTemplate>();
        }
    }
}