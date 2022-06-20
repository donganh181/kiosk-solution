using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class Kiosk
    {
        public Kiosk()
        {
            KioskSchedules = new HashSet<KioskSchedule>();
            ServiceOrders = new HashSet<ServiceOrder>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? PartyId { get; set; }
        public Guid? KioskLocationId { get; set; }
        public string Status { get; set; }
        public long? Longtitude { get; set; }
        public long? Latitude { get; set; }

        public virtual KioskLocation KioskLocation { get; set; }
        public virtual Party Party { get; set; }
        public virtual ICollection<KioskSchedule> KioskSchedules { get; set; }
        public virtual ICollection<ServiceOrder> ServiceOrders { get; set; }
    }
}
