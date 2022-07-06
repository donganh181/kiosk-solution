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
    public static class PartyNotificationModule
    {
        public static void ConfigPartyNotificationModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<PartyNotification, PartyNotificationViewModel>()
                .ForMember(src => src.PartyMail, opt => opt.MapFrom(des => des.Party.Email))
                .ForMember(src => src.NotiTitle, opt => opt.MapFrom(des => des.Notification.Title))
                .ForMember(src => src.NotiContent, opt => opt.MapFrom(des => des.Notification.Content))
                .ForMember(src => src.NotiCreateDate, opt => opt.MapFrom(des => des.Notification.CreateDate));
            mc.CreateMap<PartyNotificationViewModel, PartyNotification>();

            mc.CreateMap<PartyNotification, PartyNotificationCreateViewModel>();
            mc.CreateMap<PartyNotificationCreateViewModel, PartyNotification>();
        }
    }
}
