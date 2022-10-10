using System;
using System.Collections.Generic;

namespace kiosk_solution.Data.ViewModels
{
    public class PoiViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ImageViewModel Thumbnail { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public string DayOfWeek { get; set; }
        public string Longtitude { get; set; }
        public string Latitude { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string CreatorEmail { get; set; }
        public string Status { get; set; }
        public Guid? PoicategoryId { get; set; }
        public string PoicategoryName { get; set; }
        public string Type { get; set; }
        public List<PoiImageDetailViewModel> ListImage { get; set; }
        public string? Banner { get; set; }
    }
}