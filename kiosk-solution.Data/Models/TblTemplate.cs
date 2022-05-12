using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class TblTemplate
    {
        public TblTemplate()
        {
            TblEvents = new HashSet<TblEvent>();
            TblPositions = new HashSet<TblPosition>();
            TblScheduleTemplates = new HashSet<TblScheduleTemplate>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? PartyId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }

        public virtual TblParty Party { get; set; }
        public virtual ICollection<TblEvent> TblEvents { get; set; }
        public virtual ICollection<TblPosition> TblPositions { get; set; }
        public virtual ICollection<TblScheduleTemplate> TblScheduleTemplates { get; set; }
    }
}
