using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class ScheduleTemplate
    {
        public string Id { get; set; }
        public string ScheduleId { get; set; }
        public string TemplateId { get; set; }

        public virtual TblSchedule Schedule { get; set; }
        public virtual TblTemplate Template { get; set; }
    }
}
