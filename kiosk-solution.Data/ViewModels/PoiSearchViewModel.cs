using System;
using System.Collections.Generic;
using kiosk_solution.Data.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace kiosk_solution.Data.ViewModels
{
    public class PoiSearchViewModel
    {
        [BindNever]
        public Guid? Id { get; set; }
        public string Name { get; set; }
        [BindNever]
        public string Description { get; set; }
        [BindNever]
        public ImageViewModel Thumbnail { get; set; }
        [BindNever]
        public TimeSpan? OpenTime { get; set; }
        [BindNever]
        public TimeSpan? CloseTime { get; set; }
        [BindNever]
        public string DayOfWeek { get; set; }
        [Specific]
        public double Longtitude { get; set; }
        [Specific]
        public double Latitude { get; set; }
        
        public string Ward { get; set; }
        
        public string District { get; set; }
        
        public string City { get; set; }
        
        public string Address { get; set; }
        [BindNever]
        public DateTime? CreateDate { get; set; }
        [BindNever]
        public Guid? CreatorId { get; set; }
        [BindNever]
        public string Status { get; set; }
        
        public Guid? PoicategoryId { get; set; }
        
        public string Type { get; set; }
        [BindNever]
        public List<PoiImageDetailViewModel> ListImage { get; set; }
    }
}