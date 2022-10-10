using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class Kiosk
    {
        public Kiosk()
        {
            KioskRatings = new HashSet<KioskRating>();
            KioskScheduleTemplates = new HashSet<KioskScheduleTemplate>();
            ServiceOrders = new HashSet<ServiceOrder>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? PartyId { get; set; }
        public Guid? KioskLocationId { get; set; }
        public string Status { get; set; }
        public double? Longtitude { get; set; }
        public double? Latitude { get; set; }
        public string DeviceId { get; set; }

        public virtual KioskLocation KioskLocation { get; set; }
        public virtual Party Party { get; set; }
        public virtual ICollection<KioskRating> KioskRatings { get; set; }
        public virtual ICollection<KioskScheduleTemplate> KioskScheduleTemplates { get; set; }
        public virtual ICollection<ServiceOrder> ServiceOrders { get; set; }
    }
}
