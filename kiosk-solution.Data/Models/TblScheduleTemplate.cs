using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class TblScheduleTemplate
    {
        public Guid Id { get; set; }
        public Guid? ScheduleId { get; set; }
        public Guid? TemplateId { get; set; }

        public virtual TblSchedule Schedule { get; set; }
        public virtual TblTemplate Template { get; set; }
    }
}
