using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class ScheduleTemplate
    {
        public Guid Id { get; set; }
        public Guid? ScheduleId { get; set; }
        public Guid? TemplateId { get; set; }

        public virtual Schedule Schedule { get; set; }
        public virtual Template Template { get; set; }
    }
}
