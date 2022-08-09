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
    public static class PartyServiceApplicationModule
    {
        public static void ConfigPartyServiceApplicationModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<PartyServiceApplication, PartyServiceApplicationViewModel>()
                .ForMember(src => src.PartyName, opt => opt.MapFrom(des => des.Party.FirstName))
                .ForMember(src => src.PartyEmail, opt => opt.MapFrom(des => des.Party.Email))
                .ForMember(src => src.ServiceApplicationName, opt => opt.MapFrom(des => des.ServiceApplication.Name))
                .ForMember(src => src.ServiceApplicationDescription, opt => opt.MapFrom(des => des.ServiceApplication.Description))
                .ForMember(src => src.ServiceApplicationLogo, opt => opt.MapFrom(des => des.ServiceApplication.Logo))
                .ForMember(src => src.ServiceApplicationLink, opt => opt.MapFrom(des => des.ServiceApplication.Link))
                .ForMember(src => src.AppCategoryId, opt => opt.MapFrom(des => des.ServiceApplication.AppCategory.Id))
                .ForMember(src => src.AppcategoryName, opt => opt.MapFrom(des => des.ServiceApplication.AppCategory.Name));
            mc.CreateMap<PartyServiceApplicationViewModel, PartyServiceApplication>();

            mc.CreateMap<PartyServiceApplication, PartyServiceApplicationCreateViewModel>();
            mc.CreateMap<PartyServiceApplicationCreateViewModel, PartyServiceApplication>();

            mc.CreateMap<PartyServiceApplication, MyAppViewModel>()
                .ForMember(src => src.ServiceAppModel, opt => opt.MapFrom(des => des.ServiceApplication));
            mc.CreateMap<MyAppViewModel, PartyServiceApplication>();

            mc.CreateMap<PartyServiceApplication, PartyServiceApplicationSearchViewModel>()
                .ForMember(src => src.PartyName, opt => opt.MapFrom(des => des.Party.FirstName))
                .ForMember(src => src.PartyEmail, opt => opt.MapFrom(des => des.Party.Email))
                .ForMember(src => src.ServiceApplicationName, opt => opt.MapFrom(des => des.ServiceApplication.Name))
                .ForMember(src => src.ServiceApplicationDescription, opt => opt.MapFrom(des => des.ServiceApplication.Description))
                .ForMember(src => src.ServiceApplicationLogo, opt => opt.MapFrom(des => des.ServiceApplication.Logo))
                .ForMember(src => src.ServiceApplicationLink, opt => opt.MapFrom(des => des.ServiceApplication.Link))
                .ForMember(src => src.AppCategoryId, opt => opt.MapFrom(des => des.ServiceApplication.AppCategory.Id))
                .ForMember(src => src.AppcategoryName, opt => opt.MapFrom(des => des.ServiceApplication.AppCategory.Name));
            mc.CreateMap<PartyServiceApplicationSearchViewModel, PartyServiceApplication>();
        }
    }
}
