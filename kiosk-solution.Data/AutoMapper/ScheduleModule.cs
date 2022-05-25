using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Data.AutoMapper
{
    public static class ScheduleModule
    {
        public static void ConfigScheduleModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Schedule, CreateScheduleViewModel>();
            mc.CreateMap<CreateScheduleViewModel, Schedule>();
            
            mc.CreateMap<Schedule, ScheduleViewModel>();
            mc.CreateMap<ScheduleViewModel, Schedule>();
        }
    }
}