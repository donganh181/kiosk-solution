using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class KioskSchedule
    {
        public Guid Id { get; set; }
        public Guid? KioskId { get; set; }
        public Guid? ScheduleId { get; set; }

        public virtual Kiosk Kiosk { get; set; }
        public virtual Schedule Schedule { get; set; }
    }
}
