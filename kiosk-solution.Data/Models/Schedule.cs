using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class Schedule
    {
        public Schedule()
        {
            KioskScheduleTemplates = new HashSet<KioskScheduleTemplate>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public TimeSpan? TimeStart { get; set; }
        public TimeSpan? TimeEnd { get; set; }
        public string DayOfWeek { get; set; }
        public Guid? PartyId { get; set; }
        public string Status { get; set; }

        public virtual Party Party { get; set; }
        public virtual ICollection<KioskScheduleTemplate> KioskScheduleTemplates { get; set; }
    }
}
