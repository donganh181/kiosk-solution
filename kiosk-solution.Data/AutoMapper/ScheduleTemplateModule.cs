using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Data.AutoMapper
{
    public static class ScheduleTemplateModule
    {
        public static void ConfigScheduleTemplateModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<ScheduleTemplate, AddTemplateViewModel>();
            mc.CreateMap<AddTemplateViewModel, ScheduleTemplate>();

            mc.CreateMap<ScheduleTemplate, ScheduleTemplateViewModel>();
            mc.CreateMap<ScheduleTemplateViewModel, ScheduleTemplate>();
        }
    }
}