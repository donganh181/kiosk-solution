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
    public static class ServiceApplicationPublishRequestModule
    {
        public static void ConfigServiceApplicationPublishRequestModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<ServiceApplicationPublishRequest, ServiceApplicationPublishRequestViewModel>()
                .ForMember(src => src.CreatorName, opt => opt.MapFrom(des => des.Creator.FirstName))
                .ForMember(src => src.CreatorEmail, opt => opt.MapFrom(des => des.Creator.Email))
                .ForMember(src => src.HandlerName, opt => opt.MapFrom(des => des.Handler.FirstName))
                .ForMember(src => src.HandlerEmail, opt => opt.MapFrom(des => des.Handler.Email))
                .ForMember(src => src.ServiceApplicationName, opt => opt.MapFrom(des => des.ServiceApplication.Name));
            mc.CreateMap<ServiceApplicationPublishRequestViewModel, ServiceApplicationPublishRequest>();

            mc.CreateMap<ServiceApplicationPublishRequest, ServiceApplicationPublishRequestCreateViewModel>();
            mc.CreateMap<ServiceApplicationPublishRequestCreateViewModel, ServiceApplicationPublishRequest>();

            mc.CreateMap<ServiceApplicationPublishRequest, UpdateServiceApplicationPublishRequestViewModel>();
            mc.CreateMap<UpdateServiceApplicationPublishRequestViewModel, ServiceApplicationPublishRequest>();

            mc.CreateMap<ServiceApplicationPublishRequest, ServiceApplicationPublishRequestSearchViewModel>()
                .ForMember(src => src.CreatorName, opt => opt.MapFrom(des => des.Creator.FirstName))
                .ForMember(src => src.CreatorEmail, opt => opt.MapFrom(des => des.Creator.Email))
                .ForMember(src => src.HandlerName, opt => opt.MapFrom(des => des.Handler.FirstName))
                .ForMember(src => src.HandlerEmail, opt => opt.MapFrom(des => des.Handler.Email))
                .ForMember(src => src.ServiceApplicationName, opt => opt.MapFrom(des => des.ServiceApplication.Name));
            mc.CreateMap<ServiceApplicationPublishRequestSearchViewModel, ServiceApplicationPublishRequest>();

        }
    }
}
