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
            Schedules = new HashSet<Schedule>();
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
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
