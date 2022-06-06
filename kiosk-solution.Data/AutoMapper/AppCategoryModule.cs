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
    public static class AppCategoryModule
    {
        public static void ConfigAppCategoryModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<AppCategory, AppCategoryViewModel>();
            mc.CreateMap<AppCategoryViewModel, AppCategory>();

            mc.CreateMap<AppCategory, AppCategoryCreateViewModel>();
            mc.CreateMap<AppCategoryCreateViewModel, AppCategory>();

            mc.CreateMap<AppCategory, AppCategoryUpdateViewModel>();
            mc.CreateMap<AppCategoryUpdateViewModel, AppCategory>();

            mc.CreateMap<AppCategory, AppCategorySearchViewModel>();
            mc.CreateMap<AppCategorySearchViewModel, AppCategory>();
        }
    }
}
