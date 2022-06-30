using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class Template
    {
        public Template()
        {
            AppCategoryPositions = new HashSet<AppCategoryPosition>();
            EventPositions = new HashSet<EventPosition>();
            KioskScheduleTemplates = new HashSet<KioskScheduleTemplate>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? PartyId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }

        public virtual Party Party { get; set; }
        public virtual ICollection<AppCategoryPosition> AppCategoryPositions { get; set; }
        public virtual ICollection<EventPosition> EventPositions { get; set; }
        public virtual ICollection<KioskScheduleTemplate> KioskScheduleTemplates { get; set; }
    }
}
