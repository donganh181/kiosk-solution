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
    public static class KioskRatingModule
    {
        public static void ConfigKioskRatingModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<KioskRating, KioskRatingViewModel>();
            mc.CreateMap<KioskRatingViewModel, KioskRating>();

            mc.CreateMap<KioskRating, KioskRatingCreateViewModel>();
            mc.CreateMap<KioskRatingCreateViewModel, KioskRating>();
        }
    }
}
