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
    public static class ServiceApplicationFeedBackModule
    {
        public static void ConfigServiceApplicationFeedBackModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<ServiceApplicationFeedBack, ServiceApplicationFeedBackViewModel>()
                .ForMember(src => src.PartyEmail, opt => opt.MapFrom(des => des.Party.Email))
                .ForMember(src => src.ServiceApplicationName, opt => opt.MapFrom(des => des.ServiceApplication.Name));
            mc.CreateMap<ServiceApplicationFeedBackViewModel, ServiceApplicationFeedBack>();

            mc.CreateMap<ServiceApplicationFeedBack, ServiceApplicationFeedBackCreateViewModel>();
            mc.CreateMap<ServiceApplicationFeedBackCreateViewModel, ServiceApplicationFeedBack>();
            
            mc.CreateMap<ServiceApplicationFeedBack, ServiceApplicationFeedBackUpdateViewModel>();
            mc.CreateMap<ServiceApplicationFeedBackUpdateViewModel, ServiceApplicationFeedBack>();
        }
    }
}
