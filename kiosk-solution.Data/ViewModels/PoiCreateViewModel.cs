using System;
using System.Collections.Generic;

namespace kiosk_solution.Data.ViewModels
{
    public class PoiCreateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string StringOpenTime { get; set; }
        public string StringCloseTime { get; set; }
        public string DayOfWeek { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public Guid? PoicategoryId { get; set; }
        public string Thumbnail { get; set; }
        public List<string> ListImage { get; set; }
        public string? Banner { get; set; }

    }
}