using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class KioskScheduleTemplate
    {
        public Guid Id { get; set; }
        public Guid? ScheduleId { get; set; }
        public Guid? TemplateId { get; set; }
        public Guid? KioskId { get; set; }
        public string Status { get; set; }

        public virtual Kiosk Kiosk { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual Template Template { get; set; }
    }
}
