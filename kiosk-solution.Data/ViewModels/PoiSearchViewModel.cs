using System;
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
        public TimeSpan? OpenTime { get; set; }
        [BindNever]
        public TimeSpan? CloseTime { get; set; }
        [BindNever]
        public string DayOfWeek { get; set; }
        [BindNever]
        public string Longtitude { get; set; }
        [BindNever]
        public string Latitude { get; set; }
        [String]
        public string Ward { get; set; }
        [String]
        public string District { get; set; }
        [String]
        public string City { get; set; }
        [String]
        public string Address { get; set; }
        [BindNever]
        public DateTime? CreateDate { get; set; }
        [BindNever]
        public Guid? CreatorId { get; set; }
        [BindNever]
        public string Status { get; set; }
        [Guid]
        public Guid? PoicategoryId { get; set; }
        [BindNever]
        public string Type { get; set; }
    }
}