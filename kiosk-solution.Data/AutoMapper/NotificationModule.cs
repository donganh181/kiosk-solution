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
    public static class NotificationModule
    {
        public static void ConfigNotificationModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Notification, NotificationViewModel>();
            mc.CreateMap<NotificationViewModel, Notification>();

            mc.CreateMap<Notification, NotificationCreateViewModel>();
            mc.CreateMap<NotificationCreateViewModel, Notification>();
        }
    }
}
