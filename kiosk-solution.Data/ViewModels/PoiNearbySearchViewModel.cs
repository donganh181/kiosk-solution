using kiosk_solution.Data.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class PoiNearbySearchViewModel
    {
        [BindNever]
        public Guid? Id { get; set; }
        [String]
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
        [BindNever]
        public string Ward { get; set; }
        [BindNever]
        public string District { get; set; }
        [BindNever]
        public string City { get; set; }
        [String]
        public string Address { get; set; }
        [BindNever]
        public DateTime? CreateDate { get; set; }
        [BindNever]
        public Guid? CreatorId { get; set; }
        [BindNever]
        public string CreatorName { get; set; }
        [BindNever]
        public string CreatorEmail { get; set; }
        [String]
        public string Status { get; set; }
        [Guid]
        public Guid? PoicategoryId { get; set; }
        [String]
        public string PoicategoryName { get; set; }
        [BindNever]
        public string Type { get; set; }
        [BindNever]
        public List<PoiImageDetailViewModel> ListImage { get; set; }
    }
}
