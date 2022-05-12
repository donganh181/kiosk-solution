using System;
using System.Collections.Generic;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class TblSchedule
    {
        public TblSchedule()
        {
            TblScheduleTemplates = new HashSet<TblScheduleTemplate>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string DayOfWeek { get; set; }
        public Guid? KioskId { get; set; }
        public Guid? PartyId { get; set; }
        public string Status { get; set; }

        public virtual TblKiosk Kiosk { get; set; }
        public virtual TblParty Party { get; set; }
        public virtual ICollection<TblScheduleTemplate> TblScheduleTemplates { get; set; }
    }
}
