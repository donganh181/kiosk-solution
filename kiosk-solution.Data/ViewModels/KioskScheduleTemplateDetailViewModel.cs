using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class KioskScheduleTemplateDetailViewModel
    {
        public Guid Id { get; set; }

        //Schedule Zone
        public Guid ScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public string DayOfWeek { get; set; }

        //Template zone
        public Guid TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateDescription { get; set; }

        //Kiosk zone
        public Guid KioskId { get; set; }
        public string DeviceId { get; set; }
    }
}
