using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class Template
    {
        public Template()
        {
            Events = new HashSet<Event>();
            Positions = new HashSet<Position>();
            ScheduleTemplates = new HashSet<ScheduleTemplate>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? PartyId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }

        public virtual Party Party { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
        public virtual ICollection<ScheduleTemplate> ScheduleTemplates { get; set; }
    }
}
