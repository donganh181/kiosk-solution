using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;

namespace kiosk_solution.Data.AutoMapper
{
    public static class EventPositionModule
    {
        public static void ConfigEventPositionModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<EventPosition, EventPositionViewModel>()
                .ForMember(src => src.TemplateName, opt => opt.MapFrom(des => des.Template.Name));
            mc.CreateMap<EventPositionViewModel, EventPosition>();

            mc.CreateMap<EventPosition, EventPositionCreateViewModel>();
            mc.CreateMap<EventPositionCreateViewModel, EventPosition>();
            
            mc.CreateMap<EventPosition, EventPositionDetailUpdateViewModel>();
            mc.CreateMap<EventPositionDetailUpdateViewModel, EventPosition>();

            mc.CreateMap<EventPosition, EventPositionDetailCreateViewModel>();
            mc.CreateMap<EventPositionDetailCreateViewModel, EventPosition>();

            mc.CreateMap<EventPosition, EventPositionSpecificViewModel>()
                .ForMember(src => src.EventType, opt => opt.MapFrom(des => des.Event.Type))
                .ForMember(src => src.EventStatus, opt => opt.MapFrom(des => des.Event.Status))
                .ForMember(src => src.Description, opt => opt.MapFrom(des => des.Event.Description))
                .ForMember(src => src.Address, opt => opt.MapFrom(des => des.Event.Address));
            mc.CreateMap<EventPositionSpecificViewModel, EventPosition>();

            mc.CreateMap<EventPosition, EventPositionDetailViewModel>()
                .ForMember(src => src.EventName, opt => opt.MapFrom(des => des.Event.Name))
                .ForMember(src => src.TemplateName, opt => opt.MapFrom(des => des.Template.Name));
            mc.CreateMap<EventPositionViewModel, EventPosition>();
            
            mc.CreateMap<EventPosition, EventPositionDetailGetViewModel>()
                .ForMember(src => src.EventName, opt => opt.MapFrom(des => des.Event.Name));
            mc.CreateMap<EventPositionDetailGetViewModel, EventPosition>();
        }
    }
}