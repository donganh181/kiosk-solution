using System;
using kiosk_solution.Data.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace kiosk_solution.Data.ViewModels
{
    public class PoiCategorySearchViewModel
    {
        [BindNever]
        public Guid? Id { get; set; }
        [String]
        public string Name { get; set; }
        [BindNever]
        public string Logo { get; set; }
    }
}