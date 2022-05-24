using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class Schedule
    {
        public Schedule()
        {
            KioskSchedules = new HashSet<KioskSchedule>();
            ScheduleTemplates = new HashSet<ScheduleTemplate>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public TimeSpan? TimeStart { get; set; }
        public TimeSpan? TimeEnd { get; set; }
        public string DayOfWeek { get; set; }
        public Guid? PartyId { get; set; }
        public string Status { get; set; }

        public virtual Party Party { get; set; }
        public virtual ICollection<KioskSchedule> KioskSchedules { get; set; }
        public virtual ICollection<ScheduleTemplate> ScheduleTemplates { get; set; }
    }
}
