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
    public static class AppCategoryPositionModule
    {
        public static void ConfigAppCategoryPositionModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<AppCategoryPosition, AppCategoryPositionViewModel>()
                .ForMember(src => src.TemplateName, opt => opt.MapFrom(des => des.Template.Name));
            mc.CreateMap<AppCategoryPositionViewModel, AppCategoryPosition>();

            mc.CreateMap<AppCategoryPosition, AppCategoryPositionCreateViewModel>();
            mc.CreateMap<AppCategoryPositionCreateViewModel, AppCategoryPosition>();

            mc.CreateMap<AppCategoryPosition, CategoryPositionDetailCreateViewModel>();
            mc.CreateMap<CategoryPositionDetailCreateViewModel, AppCategoryPosition>();

            mc.CreateMap<AppCategoryPosition, CategoryPositionDetailUpdateViewModel>();
            mc.CreateMap<CategoryPositionDetailUpdateViewModel, AppCategoryPosition>();

            mc.CreateMap<AppCategoryPosition, AppCategoryPositionSpecificViewModel>()
                .ForMember(src => src.AppCategoryLogo, opt => opt.MapFrom(des => des.AppCategory.Logo))
                .ForMember(src => src.AppCategoryName, opt => opt.MapFrom(des => des.AppCategory.Name));
            mc.CreateMap<AppCategoryPositionSpecificViewModel, AppCategoryPosition>();

            mc.CreateMap<AppCategoryPosition, CategoryPositionDetailViewModel>()
                .ForMember(src => src.AppCategoryName, opt => opt.MapFrom(des => des.AppCategory.Name));
            mc.CreateMap<AppCategoryPositionViewModel, AppCategoryPosition>();

            mc.CreateMap<AppCategoryPosition, AppCategoryPositionDetailViewModel>()
                .ForMember(src => src.AppCategoryName, opt => opt.MapFrom(des => des.AppCategory.Name));
            mc.CreateMap<AppCategoryPositionDetailViewModel, AppCategoryPosition>();
        }
    }
}