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
    public static class TemplateModule
    {
        public static void ConfigTemplateModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Template, TemplateViewModel>();
            mc.CreateMap<TemplateViewModel, Template>();

            mc.CreateMap<Template, TemplateCreateViewModel>();
            mc.CreateMap<TemplateCreateViewModel, Template>();

            mc.CreateMap<Template, TemplateSearchViewModel>();
            mc.CreateMap<TemplateSearchViewModel, Template>();

            mc.CreateMap<Template, TemplateDetailViewModel>()
                .ForMember(src => src.ListAppCatePosition, opt => opt.MapFrom(des => des.AppCategoryPositions.ToList()))
                .ForMember(src => src.ListEventPosition, opt => opt.MapFrom(des => des.EventPositions.ToList()));
            mc.CreateMap<TemplateDetailViewModel, Template>();
        }
    }
}
