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
    public static class ImageModule
    {
        public static void ConfigImageModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Image, ImageViewModel>();
            mc.CreateMap<ImageViewModel, Image>();

            mc.CreateMap<Image, ImageCreateViewModel>();
            mc.CreateMap<ImageCreateViewModel, Image>();

            mc.CreateMap<Image, ImageSearchViewModel>();
            mc.CreateMap<ImageSearchViewModel, Image>();

            mc.CreateMap<Image, ImageUpdateViewModel>();
            mc.CreateMap<ImageUpdateViewModel, Image>();

            mc.CreateMap<ImageViewModel, EventImageDetailViewModel>();
            mc.CreateMap<EventImageDetailViewModel, ImageViewModel>();

            mc.CreateMap<ImageViewModel, PoiImageDetailViewModel>();
            mc.CreateMap<PoiImageDetailViewModel, ImageViewModel>();
        }
    }
}
