using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Data.AutoMapper
{
    public static class KioskScheduleTemplateModule
    {
        public static void ConfigScheduleTemplateModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<KioskScheduleTemplate, KioskScheduleTemplateCreateViewModel>();
            mc.CreateMap<KioskScheduleTemplateCreateViewModel, ScheduleTemplate>();

            mc.CreateMap<KioskScheduleTemplate, KioskScheduleTemplateViewModel>();
            mc.CreateMap<KioskScheduleTemplateViewModel, KioskScheduleTemplate>();
        }
    }
}