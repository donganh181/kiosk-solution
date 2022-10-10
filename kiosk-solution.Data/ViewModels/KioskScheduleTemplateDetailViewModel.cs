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
        public ScheduleViewModel Schedule { get; set; }
        public TemplateDetailViewModel Template { get; set; }
    }
}
