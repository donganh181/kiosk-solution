using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.AutoMapper
{
    public static class EventModule
    {
        public static void ConfigEventModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Event, EventViewModel>()
                .ForMember(src => src.CreatorName, opt => opt.MapFrom(des => des.Creator.FirstName))
                .ForMember(src => src.CreatorEmail, opt => opt.MapFrom(des => des.Creator.Email));
            mc.CreateMap<EventViewModel, Event>();

            mc.CreateMap<Event, EventCreateViewModel>();
            mc.CreateMap<EventCreateViewModel, Event>();

            mc.CreateMap<Event, EventSearchViewModel>()
                .ForMember(src => src.CreatorName, opt => opt.MapFrom(des => des.Creator.FirstName))
                .ForMember(src => src.CreatorEmail, opt => opt.MapFrom(des => des.Creator.Email));
            mc.CreateMap<EventSearchViewModel, Event>();

            mc.CreateMap<Event, EventNearbySearchViewModel>()
                .ForMember(src => src.CreatorName, opt => opt.MapFrom(des => des.Creator.FirstName))
                .ForMember(src => src.CreatorEmail, opt => opt.MapFrom(des => des.Creator.Email));
            mc.CreateMap<EventNearbySearchViewModel, Event>();

            mc.CreateMap<Event, EventUpdateViewModel>();
            mc.CreateMap<EventUpdateViewModel, Event>();

            mc.CreateMap<Event, EventImageViewModel>();
            mc.CreateMap<EventImageViewModel, Event>();
        }
    }
}
