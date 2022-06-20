﻿using AutoMapper;
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

            mc.CreateMap<EventPosition, EventPositionDetailViewModel>()
                .ForMember(src => src.EventName, opt => opt.MapFrom(des => des.Event.Name))
                .ForMember(src => src.TemplateName, opt => opt.MapFrom(des => des.Template.Name));
            mc.CreateMap<EventPositionViewModel, EventPosition>();
        }
    }
}