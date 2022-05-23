using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class Kiosk
    {
        public Kiosk()
        {
            Orders = new HashSet<Order>();
            KioskSchedules = new HashSet<KioskSchedule>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? PartyId { get; set; }
        public Guid? KioskLocationId { get; set; }
        public string Status { get; set; }

        public virtual KioskLocation KioskLocation { get; set; }
        public virtual Party Party { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<KioskSchedule> KioskSchedules { get; set; }

        public string Longtitude { get; set; }

        public string Latitude { get; set; }
    }
}