using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class Poi
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public string DayOfWeek { get; set; }
        public double? Longtitude { get; set; }
        public double? Latitude { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreatorId { get; set; }
        public string Status { get; set; }
        public Guid? PoicategoryId { get; set; }
        public string Type { get; set; }
        public string Banner { get; set; }

        public virtual Party Creator { get; set; }
        public virtual Poicategory Poicategory { get; set; }
    }
}
