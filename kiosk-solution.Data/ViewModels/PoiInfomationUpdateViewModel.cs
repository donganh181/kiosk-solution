using System;

namespace kiosk_solution.Data.ViewModels
{
    public class PoiInfomationUpdateViewModel
    {
        public Guid Id { get; set; }
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
        public Guid? ThumbnailId { get; set; }
        public string Thumbnail { get; set; }
    }
}